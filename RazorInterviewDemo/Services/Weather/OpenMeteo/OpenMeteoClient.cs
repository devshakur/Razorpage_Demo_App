using System.Globalization;

namespace RazorInterviewDemo.Services.Weather.OpenMeteo;

public class OpenMeteoClient(HttpClient httpClient) : IOpenMeteoClient
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    public async Task<OpenMeteoForecastResponse> GetForecastAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        var latitudeValue = latitude.ToString(Invariant);
        var longitudeValue = longitude.ToString(Invariant);

        var url =
            "forecast?" +
            $"latitude={latitudeValue}&longitude={longitudeValue}" +
            "&current=temperature_2m,apparent_temperature,relative_humidity_2m,wind_speed_10m,weather_code" +
            "&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m,weather_code" +
            "&daily=weather_code,temperature_2m_max,temperature_2m_min,relative_humidity_2m_mean,wind_speed_10m_max" +
            "&forecast_days=7&timezone=auto&wind_speed_unit=ms";

        var response = await httpClient.GetFromJsonAsync<OpenMeteoForecastResponse>(url, cancellationToken)
            ?? throw new InvalidOperationException("Weather forecast response was empty.");

        return response;
    }

    public async Task<string> GetLocationNameAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        var latitudeValue = latitude.ToString(Invariant);
        var longitudeValue = longitude.ToString(Invariant);

        var url = $"https://geocoding-api.open-meteo.com/v1/reverse?latitude={latitudeValue}&longitude={longitudeValue}&language=en";
        var response = await httpClient.GetFromJsonAsync<OpenMeteoReverseGeocodeResponse>(url, cancellationToken);

        var result = response?.Results.FirstOrDefault();
        if (result is null)
        {
            return "Unknown location";
        }

        if (!string.IsNullOrWhiteSpace(result.Admin1) &&
            !result.Admin1.Equals(result.Name, StringComparison.OrdinalIgnoreCase))
        {
            return $"{result.Name}, {result.Admin1}, {result.Country}";
        }

        return $"{result.Name}, {result.Country}";
    }
}
