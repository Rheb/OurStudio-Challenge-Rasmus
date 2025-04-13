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

