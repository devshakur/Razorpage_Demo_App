namespace RazorInterviewDemo.Models.Weather;

public class TopBarModel
{
    public string Location { get; set; } = string.Empty;
    public IList<IconButtonModel> NavButtons { get; set; } = [];
    public IconButtonModel SettingsButton { get; set; } = new();
}
