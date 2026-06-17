namespace RazorInterviewDemo.Models.Weather;

public class TemperatureChartModel
{
    public string Title { get; set; } = "Обзор температуры на неделю";
    public IList<ChartDataPointModel> Points { get; set; } = [];
}
