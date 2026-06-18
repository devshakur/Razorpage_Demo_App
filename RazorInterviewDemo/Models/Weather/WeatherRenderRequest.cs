using System.Text.Json.Serialization;
using RazorInterviewDemo.Services.Weather.OpenMeteo;

namespace RazorInterviewDemo.Models.Weather;

public sealed class WeatherRenderRequest
{
    [JsonPropertyName("forecast")]
    public OpenMeteoForecastResponse? Forecast { get; set; }

    [JsonPropertyName("locationName")]
    public string LocationName { get; set; } = string.Empty;
}
