using System.Text.Json.Serialization;

namespace RazorInterviewDemo.Services.Weather.OpenMeteo;

public sealed class OpenMeteoForecastResponse
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = string.Empty;

    [JsonPropertyName("current")]
    public OpenMeteoCurrent? Current { get; set; }

    [JsonPropertyName("hourly")]
    public OpenMeteoHourly? Hourly { get; set; }

    [JsonPropertyName("daily")]
    public OpenMeteoDaily? Daily { get; set; }
}

public sealed class OpenMeteoCurrent
{
    [JsonPropertyName("temperature_2m")]
    public double Temperature { get; set; }

    [JsonPropertyName("apparent_temperature")]
    public double ApparentTemperature { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public int RelativeHumidity { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public double WindSpeed { get; set; }

    [JsonPropertyName("weather_code")]
    public int WeatherCode { get; set; }
}

public sealed class OpenMeteoHourly
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; } = [];

    [JsonPropertyName("temperature_2m")]
    public List<double> Temperature { get; set; } = [];

    [JsonPropertyName("relative_humidity_2m")]
    public List<int> RelativeHumidity { get; set; } = [];

    [JsonPropertyName("wind_speed_10m")]
    public List<double> WindSpeed { get; set; } = [];

    [JsonPropertyName("weather_code")]
    public List<int> WeatherCode { get; set; } = [];
}

public sealed class OpenMeteoDaily
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; } = [];

    [JsonPropertyName("weather_code")]
    public List<int> WeatherCode { get; set; } = [];

    [JsonPropertyName("temperature_2m_max")]
    public List<double> TemperatureMax { get; set; } = [];

    [JsonPropertyName("temperature_2m_min")]
    public List<double> TemperatureMin { get; set; } = [];

    [JsonPropertyName("relative_humidity_2m_mean")]
    public List<int> RelativeHumidityMean { get; set; } = [];

    [JsonPropertyName("wind_speed_10m_max")]
    public List<double> WindSpeedMax { get; set; } = [];
}

public sealed class OpenMeteoReverseGeocodeResponse
{
    [JsonPropertyName("results")]
    public List<OpenMeteoGeocodeResult> Results { get; set; } = [];
}

public sealed class OpenMeteoGeocodeResult
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("admin1")]
    public string? Admin1 { get; set; }
}
