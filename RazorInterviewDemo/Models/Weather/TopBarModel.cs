namespace RazorInterviewDemo.Models.Weather;

public class TopBarModel
{
    public string Location { get; set; } = string.Empty;
    public string Greeting { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public IList<IconButtonModel> NavButtons { get; set; } = [];
}
