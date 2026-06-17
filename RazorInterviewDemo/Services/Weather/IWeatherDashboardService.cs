using RazorInterviewDemo.Models.Weather;

namespace RazorInterviewDemo.Services.Weather;

public interface IWeatherDashboardService
{
    WeatherPageViewModel GetDashboard();
}
