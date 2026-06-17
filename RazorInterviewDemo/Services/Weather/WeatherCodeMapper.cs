namespace RazorInterviewDemo.Services.Weather;

public static class WeatherCodeMapper
{
    public static string ToIcon(int weatherCode) => weatherCode switch
    {
        0 => "sunny",
        1 or 2 => "partly-cloudy",
        3 => "cloudy",
        45 or 48 => "cloudy",
        >= 51 and <= 67 => "rain",
        >= 71 and <= 77 => "cloudy",
        >= 80 and <= 82 => "rain",
        >= 95 and <= 99 => "rain",
        _ => "partly-cloudy"
    };
}
