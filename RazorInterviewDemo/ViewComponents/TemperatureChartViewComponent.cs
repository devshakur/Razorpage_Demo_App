using Microsoft.AspNetCore.Mvc;
using RazorInterviewDemo.Models.Weather;

namespace RazorInterviewDemo.ViewComponents;

public class TemperatureChartViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(TemperatureChartModel model)
    {
        return View(model);
    }
}
