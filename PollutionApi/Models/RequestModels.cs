using System.Globalization;

namespace PollutionApi.Models;

public record PollutionApiRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate_Inclusive { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string[] MeasurmentTypes { get; set; } = [];

    public string GetUrl()
    {
        return $"https://air-quality-api.open-meteo.com/v1/air-quality?" +
            $"latitude={ToUrlString(Latitude)}" +
            $"&longitude={ToUrlString(Longitude)}" +
            $"&hourly={string.Join(",", MeasurmentTypes)}" +
            $"&start_date={StartDate:yyyy-MM-dd}" +
            $"&end_date={EndDate_Inclusive:yyyy-MM-dd}"
        ;
    }

    public string ToUrlString(double val) => val.ToString(CultureInfo.InvariantCulture);
}

