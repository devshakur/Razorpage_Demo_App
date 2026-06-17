namespace RazorInterviewDemo.Services.Weather.OpenMeteo;

public interface IOpenMeteoClient
{
    Task<OpenMeteoForecastResponse> GetForecastAsync(double latitude, double longitude, CancellationToken cancellationToken = default);

    Task<string> GetLocationNameAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
}
