using Microsoft.AspNetCore.Mvc;
using RazorInterviewDemo.Models.Weather;

namespace RazorInterviewDemo.ViewComponents;

public class IconButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IconButtonModel model)
    {
        return View(model);
    }
}
