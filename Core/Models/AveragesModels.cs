namespace Core.Models;

public class DailyPMAverages
{
    public Averages Pm10 { get; set; } = new();
    public Averages Pm2pt5 { get; set; } = new();

    public const string Unit = "μg / m³";

    public string GetSummary()
    {
        return
            $"PM-10 averages:" +
            $"\n{Pm10.GetSummary(Unit)}" +
            $"\n\nPM-2.5 averages:" +
            $"\n{Pm2pt5.GetSummary(Unit)}"
        ;
    }
}


public class Averages
{
    public double Morning { get; set; }
    public double Afternoon { get; set; }
    public double Night { get; set; }

    public string GetSummary(string unit)
    {
        return
            $"\tMorning: {Morning:0.00} {unit}" +
            $"\n\tAfternoon: {Afternoon:0.00} {unit}" +
            $"\n\tNight: {Night:0.00} {unit}"
        ;
    }
}

public enum TimeGrouping
{
    /// <summary>
    /// From 06:00 to 12:00 (exclusive)
    /// </summary>
    Morning_06to12 = 0,
    /// <summary>
    /// From 12:00 to 18:00 (exclusive)
    /// </summary>
    Afternoon_12to18 = 1,
    /// <summary>
    /// From 18:00 to 06:00 (next day exclusive)
    /// </summary>
    Night_18to06 = 2,
}

public static class AveragesExtensions
{
    public static TimeGrouping GetTimeGrouping(this DateTime date)
    {
        return date.Hour switch
        {
            >= 6 and < 12 => TimeGrouping.Morning_06to12,
            >= 12 and < 18 => TimeGrouping.Afternoon_12to18,
            _ => TimeGrouping.Night_18to06
        };
    }
}