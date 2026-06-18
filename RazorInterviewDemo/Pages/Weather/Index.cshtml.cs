using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorInterviewDemo.Pages.Weather;

public class IndexModel : PageModel
{
    public bool IsLoading { get; private set; }

    public bool AwaitingWeatherData { get; private set; }

    public void OnGet(double? latitude, double? longitude)
    {
        if (latitude is null || longitude is null)
        {
            IsLoading = true;
            return;
        }

        AwaitingWeatherData = true;
    }
}
