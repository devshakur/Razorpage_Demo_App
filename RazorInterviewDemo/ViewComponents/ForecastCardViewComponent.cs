using Microsoft.AspNetCore.Mvc;
using RazorInterviewDemo.Models.Weather;

namespace RazorInterviewDemo.ViewComponents;

public class ForecastCardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ForecastCardModel model)
    {
        return View(model);
    }
}
