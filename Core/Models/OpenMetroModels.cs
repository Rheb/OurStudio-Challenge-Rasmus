using System.Text.Json.Serialization;

namespace Core.Models;

public class HourlyData
{
    [JsonPropertyName("hourly")]
    public Hourly Hourly { get; set; }
}

public class Hourly
{
    [JsonPropertyName("time")]
    public string[] Time { get; set; }

    [JsonPropertyName("pm10")]
    public double[] Pm10 { get; set; }

    [JsonPropertyName("pm2_5")]
    public double[] Pm2pt5 { get; set; }
}
