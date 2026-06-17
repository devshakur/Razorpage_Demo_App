using Microsoft.AspNetCore.Mvc;
using RazorInterviewDemo.Models.Weather;

namespace RazorInterviewDemo.ViewComponents;

public class TopBarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(TopBarModel model)
    {
        return View(model);
    }
}
