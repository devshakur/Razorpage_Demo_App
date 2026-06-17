namespace RazorInterviewDemo.Models.Weather;

public class ChartDataPointModel
{
    public string Label { get; set; } = string.Empty;
    public int Average { get; set; }
    public int Maximum { get; set; }
    public int Minimum { get; set; }
    public bool IsHighlighted { get; set; }
}
