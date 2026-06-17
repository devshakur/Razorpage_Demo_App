using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorInterviewDemo.Models.Weather;
using RazorInterviewDemo.Services.Weather;

namespace RazorInterviewDemo.Pages.Weather;

public class IndexModel : PageModel
{
    private readonly IWeatherDashboardService _weatherService;

    public IndexModel(IWeatherDashboardService weatherService)
    {
        _weatherService = weatherService;
    }

    public WeatherPageViewModel Dashboard { get; private set; } = new();

    public void OnGet()
    {
        Dashboard = _weatherService.GetDashboard();
    }
}
