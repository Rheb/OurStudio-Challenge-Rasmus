using Core.Models;
using System.Globalization;

namespace PollutionApi.Models;

public class DailyPMAverageResponse
{
    public bool IsSuccess { get; init; } = true;
    public string ErrorMessage { get; init; } = "";

    public Averages Pm10 { get; init; } = new();
    public Averages Pm2pt5 { get; init; } = new();
    public string Summary { get; init; } = "";
}
