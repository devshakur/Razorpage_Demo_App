namespace RazorInterviewDemo.Models.Weather;

public class WeatherPageViewModel
{
    public TopBarModel TopBar { get; set; } = new();
    public CurrentWeatherModel CurrentWeather { get; set; } = new();
    public ForecastCardModel Forecast { get; set; } = new();
    public TemperatureChartModel Chart { get; set; } = new();
}
