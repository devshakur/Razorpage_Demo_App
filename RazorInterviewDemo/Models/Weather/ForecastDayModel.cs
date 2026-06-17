namespace RazorInterviewDemo.Models.Weather;

public class ForecastDayModel
{
    public string DayName { get; set; } = string.Empty;
    public string ConditionIcon { get; set; } = "sunny";
    public int HighTemp { get; set; }
    public int LowTemp { get; set; }
    public int Humidity { get; set; }
    public int WindSpeed { get; set; }
}
