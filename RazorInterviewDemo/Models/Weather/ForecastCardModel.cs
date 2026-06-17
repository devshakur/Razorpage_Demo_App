namespace RazorInterviewDemo.Models.Weather;

public class ForecastCardModel
{
    public string TodayLabel { get; set; } = string.Empty;
    public string WeekLabel { get; set; } = "Неделя";
    public bool IsWeekView { get; set; } = true;
    public IList<ForecastDayModel> Days { get; set; } = [];
}
