namespace RazorInterviewDemo.Models;

public class ButtonModel
{
    public string Text { get; set; } = "Click me";
    public string Url { get; set; } = "#";
    public string CssClass { get; set; } = "btn-primary";
    public bool OpenInNewTab { get; set; }
}
