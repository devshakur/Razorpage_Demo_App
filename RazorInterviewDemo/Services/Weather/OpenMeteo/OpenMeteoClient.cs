using System.Globalization;
using System.Net.Http.Json;

namespace RazorInterviewDemo.Services.Weather.OpenMeteo;

public class OpenMeteoClient(HttpClient httpClient, IHttpClientFactory httpClientFactory) : IOpenMeteoClient
{
    private const int MaxAttempts = 3;
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    public async Task<OpenMeteoForecastResponse> GetForecastAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteWithRetryAsync(
            async token =>
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

                var response = await httpClient.GetFromJsonAsync<OpenMeteoForecastResponse>(url, token)
                    ?? throw new InvalidOperationException("Weather forecast response was empty.");

                return response;
            },
            cancellationToken);
    }

    public async Task<string> GetLocationNameAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteWithRetryAsync(
            async token =>
            {
                var latitudeValue = latitude.ToString(Invariant);
                var longitudeValue = longitude.ToString(Invariant);

                var url =
                    $"https://nominatim.openstreetmap.org/reverse?lat={latitudeValue}&lon={longitudeValue}" +
                    "&format=json&addressdetails=1&accept-language=en";

                var nominatimClient = httpClientFactory.CreateClient("Nominatim");
                using var response = await nominatimClient.GetAsync(url, token);
                if (!response.IsSuccessStatusCode)
                {
                    return FormatCoordinates(latitude, longitude);
                }

                var geocodeResponse = await response.Content.ReadFromJsonAsync<NominatimReverseGeocodeResponse>(
                    cancellationToken: token);

                var address = geocodeResponse?.Address;
                if (address is null)
                {
                    return FormatCoordinates(latitude, longitude);
                }

                var city = address.City ?? address.Town ?? address.Village ?? address.State ?? string.Empty;
                var country = address.Country ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(city) && !string.IsNullOrWhiteSpace(country))
                {
                    return $"{city}, {country}";
                }

                if (!string.IsNullOrWhiteSpace(geocodeResponse?.DisplayName))
                {
                    return geocodeResponse.DisplayName;
                }

                return FormatCoordinates(latitude, longitude);
            },
            cancellationToken);
    }

    private static async Task<T> ExecuteWithRetryAsync<T>(
        Func<CancellationToken, Task<T>> action,
        CancellationToken cancellationToken)
    {
        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                return await action(cancellationToken);
            }
            catch (Exception exception) when (
                IsTransient(exception) &&
                attempt < MaxAttempts &&
                !cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500 * attempt), cancellationToken);
            }
        }

        throw new InvalidOperationException("Retry loop exited unexpectedly.");
    }

    private static bool IsTransient(Exception exception) =>
        exception is TaskCanceledException or HttpRequestException;

    private static string FormatCoordinates(double latitude, double longitude) =>
        $"{latitude.ToString("F2", Invariant)}, {longitude.ToString("F2", Invariant)}";
}
