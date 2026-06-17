using RazorInterviewDemo.Models.Weather;

namespace RazorInterviewDemo.Services.Weather;

public interface IWeatherDashboardService
{
    Task<WeatherPageViewModel> GetDashboardAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default);
}
