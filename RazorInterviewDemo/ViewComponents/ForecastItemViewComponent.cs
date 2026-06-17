using Microsoft.AspNetCore.Mvc;
using RazorInterviewDemo.Models.Weather;

namespace RazorInterviewDemo.ViewComponents;

public class ForecastItemViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ForecastDayModel model)
    {
        return View(model);
    }
}
