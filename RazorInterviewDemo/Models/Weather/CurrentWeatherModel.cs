namespace RazorInterviewDemo.Models.Weather;

public class CurrentWeatherModel
{
    public int Temperature { get; set; }
    public string ConditionIcon { get; set; } = "partly-cloudy";
    public int FeelsLike { get; set; }
    public int Humidity { get; set; }
    public int WindSpeed { get; set; }
}
