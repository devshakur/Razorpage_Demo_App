using System.Globalization;
using RazorInterviewDemo.Models.Weather;
using RazorInterviewDemo.Services.Weather.OpenMeteo;

namespace RazorInterviewDemo.Services.Weather;

public class WeatherDashboardService(
    IOpenMeteoClient openMeteoClient,
    ILogger<WeatherDashboardService> logger) : IWeatherDashboardService
{
    private static readonly CultureInfo EnglishCulture = CultureInfo.GetCultureInfo("en-US");
    private static readonly TimeSpan ApiTimeout = TimeSpan.FromSeconds(55);

    public async Task<WeatherPageViewModel> GetDashboardAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        using var timeoutCts = new CancellationTokenSource(ApiTimeout);
        var apiToken = timeoutCts.Token;

        var forecast = await openMeteoClient.GetForecastAsync(latitude, longitude, apiToken);

        string locationName;
        try
        {
            locationName = await openMeteoClient.GetLocationNameAsync(latitude, longitude, apiToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            logger.LogWarning(
                exception,
                "Location lookup failed for {Latitude}, {Longitude}; using coordinates",
                latitude,
                longitude);
            locationName = FormatCoordinates(latitude, longitude);
        }
        var current = forecast.Current ?? throw new InvalidOperationException("Current weather data is unavailable.");

        var currentTemperature = (int)Math.Round(current.Temperature);
        var today = GetTodayDate(forecast);

        return new WeatherPageViewModel
        {
            TopBar = BuildTopBar(locationName, currentTemperature),
            CurrentWeather = new CurrentWeatherModel
            {
                Temperature = currentTemperature,
                ConditionIcon = MapIcon(current.WeatherCode, large: true),
                FeelsLike = (int)Math.Round(current.ApparentTemperature),
                Humidity = current.RelativeHumidity,
                WindSpeed = (int)Math.Round(current.WindSpeed)
            },
            Forecast = BuildForecastCard(forecast, today),
            Chart = BuildChart(forecast, today)
        };
    }

    private static TopBarModel BuildTopBar(string locationName, int currentTemperature)
    {
        return new TopBarModel
        {
            Location = $"{locationName} {currentTemperature}°C",
            NavButtons =
            [
                new IconButtonModel { Text = "Weather", Icon = "cloud", IsActive = true, Action = "nav-weather" }
            ],
            SettingsButton = new IconButtonModel
            {
                Icon = "settings",
                IsCircular = true,
                Action = "theme-cycle"
            }
        };
    }

    private static ForecastCardModel BuildForecastCard(OpenMeteoForecastResponse forecast, DateOnly today)
    {
        var card = new ForecastCardModel
        {
            TodayDateLabel = today.ToString("MMMM d", EnglishCulture),
            TodayHours = BuildTodayHours(forecast, today),
            WeekDays = BuildWeekDays(forecast, today)
        };

        return card;
    }

    private static IList<ForecastDayModel> BuildTodayHours(OpenMeteoForecastResponse forecast, DateOnly today)
    {
        var hourly = forecast.Hourly;
        if (hourly is null || hourly.Time.Count == 0)
        {
            return [];
        }

        var hours = new List<ForecastDayModel>();

        for (var index = 0; index < hourly.Time.Count; index++)
        {
            if (!DateTime.TryParse(hourly.Time[index], out var hourTime))
            {
                continue;
            }

            var hourDate = DateOnly.FromDateTime(hourTime);
            if (hourDate != today)
            {
                continue;
            }

            var temperature = GetValue(hourly.Temperature, index, 0);
            var humidity = GetValue(hourly.RelativeHumidity, index, 0);
            var windSpeed = GetValue(hourly.WindSpeed, index, 0);
            var weatherCode = GetValue(hourly.WeatherCode, index, 0);

            hours.Add(new ForecastDayModel
            {
                DayName = hourTime.ToString("h tt", EnglishCulture),
                ConditionIcon = MapIcon(weatherCode),
                HighTemp = (int)Math.Round(temperature),
                LowTemp = (int)Math.Round(temperature),
                Humidity = humidity,
                WindSpeed = (int)Math.Round(windSpeed)
            });
        }

        return hours;
    }

    private static IList<ForecastDayModel> BuildWeekDays(OpenMeteoForecastResponse forecast, DateOnly today)
    {
        var daily = forecast.Daily;
        if (daily is null || daily.Time.Count == 0)
        {
            return [];
        }

        var days = new List<ForecastDayModel>();

        for (var index = 0; index < daily.Time.Count; index++)
        {
            if (!DateOnly.TryParse(daily.Time[index], out var dayDate))
            {
                continue;
            }

            var maxTemp = (int)Math.Round(GetValue(daily.TemperatureMax, index, 0));
            var minTemp = (int)Math.Round(GetValue(daily.TemperatureMin, index, 0));

            days.Add(new ForecastDayModel
            {
                DayName = dayDate.ToString("dddd", EnglishCulture),
                ConditionIcon = MapIcon(GetValue(daily.WeatherCode, index, 0)),
                HighTemp = maxTemp,
                LowTemp = minTemp,
                Humidity = GetValue(daily.RelativeHumidityMean, index, 0),
                WindSpeed = (int)Math.Round(GetValue(daily.WindSpeedMax, index, 0)),
                IsToday = dayDate == today
            });
        }

        return days;
    }

    private static TemperatureChartModel BuildChart(OpenMeteoForecastResponse forecast, DateOnly today)
    {
        var daily = forecast.Daily;
        var points = new List<ChartDataPointModel>();

        if (daily is not null)
        {
            for (var index = 0; index < daily.Time.Count; index++)
            {
                if (!DateOnly.TryParse(daily.Time[index], out var dayDate))
                {
                    continue;
                }

                var maximum = (int)Math.Round(GetValue(daily.TemperatureMax, index, 0));
                var minimum = (int)Math.Round(GetValue(daily.TemperatureMin, index, 0));

                points.Add(new ChartDataPointModel
                {
                    Label = dayDate.ToString("ddd, M/d", EnglishCulture),
                    Average = (int)Math.Round((maximum + minimum) / 2.0),
                    Maximum = maximum,
                    Minimum = minimum,
                    IsHighlighted = dayDate == today
                });
            }
        }

        var values = points
            .SelectMany(point => new[] { point.Average, point.Maximum, point.Minimum })
            .ToList();

        var minValue = values.Count > 0 ? values.Min() : 0;
        var maxValue = values.Count > 0 ? values.Max() : 25;
        var padding = 2;
        var scaleMin = Math.Max(0, minValue - padding);
        var scaleMax = maxValue + padding;

        if (scaleMax - scaleMin < 10)
        {
            scaleMax = scaleMin + 10;
        }

        scaleMin = (int)(Math.Floor(scaleMin / 5.0) * 5);
        scaleMax = (int)(Math.Ceiling(scaleMax / 5.0) * 5);

        return new TemperatureChartModel
        {
            Title = "Weekly temperature overview",
            Points = points,
            MinTemperature = scaleMin,
            MaxTemperature = scaleMax
        };
    }

    private static DateOnly GetTodayDate(OpenMeteoForecastResponse forecast)
    {
        if (forecast.Daily?.Time.Count > 0 &&
            DateOnly.TryParse(forecast.Daily.Time[0], out var today))
        {
            return today;
        }

        return DateOnly.FromDateTime(DateTime.Now);
    }

    private static string MapIcon(int weatherCode, bool large = false)
    {
        var icon = WeatherCodeMapper.ToIcon(weatherCode);
        return large && icon == "partly-cloudy" ? "partly-cloudy-lg" : icon;
    }

    private static T GetValue<T>(IReadOnlyList<T> values, int index, T fallback)
    {
        return index >= 0 && index < values.Count ? values[index] : fallback;
    }

    private static string FormatCoordinates(double latitude, double longitude) =>
        $"{latitude.ToString("F2", CultureInfo.InvariantCulture)}, {longitude.ToString("F2", CultureInfo.InvariantCulture)}";
}
