namespace RazorInterviewDemo.Models.Weather;

public class ForecastCardModel
{
    public string TodayDateLabel { get; set; } = string.Empty;
    public string WeekLabel { get; set; } = "Week";
    public IList<ForecastDayModel> TodayHours { get; set; } = [];
    public IList<ForecastDayModel> WeekDays { get; set; } = [];
}
