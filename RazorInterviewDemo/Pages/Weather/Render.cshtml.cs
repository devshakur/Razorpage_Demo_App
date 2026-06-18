using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorInterviewDemo.Models.Weather;
using RazorInterviewDemo.Services.Weather;

namespace RazorInterviewDemo.Pages.Weather;

[IgnoreAntiforgeryToken]
public class RenderModel(IWeatherDashboardService weatherService) : PageModel
{
    public IActionResult OnPost([FromBody] WeatherRenderRequest request)
    {
        if (request.Forecast is null || string.IsNullOrWhiteSpace(request.LocationName))
        {
            return BadRequest();
        }

        try
        {
            var dashboard = weatherService.BuildDashboardFromForecast(
                request.Forecast,
                request.LocationName);

            return Partial("_DashboardContent", dashboard);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
