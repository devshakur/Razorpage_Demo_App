using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorInterviewDemo.Models.Weather;
using RazorInterviewDemo.Services.Weather;

namespace RazorInterviewDemo.Pages.Weather;

public class IndexModel : PageModel
{
    private readonly IWeatherDashboardService _weatherService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IWeatherDashboardService weatherService, ILogger<IndexModel> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    public WeatherPageViewModel Dashboard { get; private set; } = new();

    public bool IsLoading { get; private set; }

    public bool HasError { get; private set; }

    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(
        double? latitude,
        double? longitude,
        CancellationToken cancellationToken)
    {
        if (latitude is null || longitude is null)
        {
            IsLoading = true;
            return;
        }

        try
        {
            Dashboard = await _weatherService.GetDashboardAsync(
                latitude.Value,
                longitude.Value,
                cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to load weather for {Latitude}, {Longitude}", latitude, longitude);
            HasError = true;
            ErrorMessage = "Unable to load live weather data. Please try again.";
        }
    }
}
