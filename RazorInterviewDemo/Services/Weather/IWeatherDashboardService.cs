using RazorInterviewDemo.Models.Weather;
using RazorInterviewDemo.Services.Weather.OpenMeteo;

namespace RazorInterviewDemo.Services.Weather;

public interface IWeatherDashboardService
{
    Task<WeatherPageViewModel> GetDashboardAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default);

    WeatherPageViewModel BuildDashboardFromForecast(
        OpenMeteoForecastResponse forecast,
        string locationName);
}
