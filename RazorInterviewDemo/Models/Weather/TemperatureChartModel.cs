namespace RazorInterviewDemo.Models.Weather;

public class TemperatureChartModel
{
    public string Title { get; set; } = "Weekly temperature overview";
    public IList<ChartDataPointModel> Points { get; set; } = [];
    public int MinTemperature { get; set; }
    public int MaxTemperature { get; set; } = 25;
}
