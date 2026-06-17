using RazorInterviewDemo.Models.Weather;

namespace RazorInterviewDemo.Services.Weather;

public class WeatherDashboardService : IWeatherDashboardService
{
    public WeatherPageViewModel GetDashboard()
    {
        return new WeatherPageViewModel
        {
            TopBar = new TopBarModel
            {
                Location = "Россия, Самара 20°C",
                Greeting = "Привет, Миша!",
                AvatarUrl = "https://i.pravatar.cc/80?img=12",
                NavButtons =
                [
                    new IconButtonModel { Text = "Погода", Icon = "cloud", IsActive = true },
                    new IconButtonModel { Icon = "clipboard", IsCircular = true },
                    new IconButtonModel { Icon = "car", IsCircular = true }
                ]
            },
            CurrentWeather = new CurrentWeatherModel
            {
                Temperature = 18,
                ConditionIcon = "partly-cloudy-lg",
                FeelsLike = 11,
                Humidity = 68,
                WindSpeed = 11
            },
            Forecast = new ForecastCardModel
            {
                TodayLabel = "Сегодня, 11 мая",
                IsWeekView = true,
                Days =
                [
                    new ForecastDayModel { DayName = "Понедельник", ConditionIcon = "partly-cloudy", HighTemp = 18, LowTemp = 9, Humidity = 68, WindSpeed = 11 },
                    new ForecastDayModel { DayName = "Вторник", ConditionIcon = "rain", HighTemp = 16, LowTemp = 8, Humidity = 72, WindSpeed = 9 },
                    new ForecastDayModel { DayName = "Среда", ConditionIcon = "cloudy", HighTemp = 17, LowTemp = 7, Humidity = 65, WindSpeed = 10 },
                    new ForecastDayModel { DayName = "Четверг", ConditionIcon = "sunny", HighTemp = 21, LowTemp = 12, Humidity = 55, WindSpeed = 7 },
                    new ForecastDayModel { DayName = "Пятница", ConditionIcon = "partly-cloudy", HighTemp = 19, LowTemp = 10, Humidity = 60, WindSpeed = 8 },
                    new ForecastDayModel { DayName = "Суббота", ConditionIcon = "rain", HighTemp = 15, LowTemp = 6, Humidity = 78, WindSpeed = 12 },
                    new ForecastDayModel { DayName = "Воскресенье", ConditionIcon = "sunny", HighTemp = 22, LowTemp = 13, Humidity = 50, WindSpeed = 6 }
                ]
            },
            Chart = new TemperatureChartModel
            {
                Points =
                [
                    new ChartDataPointModel { Label = "пт, 8.05", Average = 14, Maximum = 18, Minimum = 10 },
                    new ChartDataPointModel { Label = "сб, 9.05", Average = 12, Maximum = 16, Minimum = 8 },
                    new ChartDataPointModel { Label = "вс, 10.05", Average = 16, Maximum = 21, Minimum = 12 },
                    new ChartDataPointModel { Label = "пн, 11.05", Average = 15, Maximum = 18, Minimum = 11, IsHighlighted = true },
                    new ChartDataPointModel { Label = "вт, 12.05", Average = 13, Maximum = 16, Minimum = 9 },
                    new ChartDataPointModel { Label = "ср, 13.05", Average = 14, Maximum = 17, Minimum = 10 },
                    new ChartDataPointModel { Label = "чт, 14.05", Average = 17, Maximum = 21, Minimum = 12 }
                ]
            }
        };
    }
}
