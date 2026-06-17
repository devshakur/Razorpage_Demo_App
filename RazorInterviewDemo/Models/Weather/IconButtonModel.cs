namespace RazorInterviewDemo.Models.Weather;

public class IconButtonModel
{
    public string? Text { get; set; }
    public string? Icon { get; set; }
    public string Url { get; set; } = "#";
    public bool IsActive { get; set; }
    public bool IsCircular { get; set; }
    public string CssClass { get; set; } = string.Empty;
}
