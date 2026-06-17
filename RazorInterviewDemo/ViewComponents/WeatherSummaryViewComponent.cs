using Microsoft.AspNetCore.Mvc;
using RazorInterviewDemo.Models.Weather;

namespace RazorInterviewDemo.ViewComponents;

public class WeatherSummaryViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(CurrentWeatherModel model)
    {
        return View(model);
    }
}
