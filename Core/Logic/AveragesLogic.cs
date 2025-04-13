using Core.Models;

namespace Core.Logic;

public static class AveragesLogic
{
    public static DailyPMAverages GetAvgPMValues(HourlyData data)
    {
        TimeSpanAggregator pm10 = new();
        TimeSpanAggregator pm2pt5 = new();

        for (int i = 0; i < data.Hourly.Time.Length; i++)
        {
            DateTime time = DateTime.Parse(data.Hourly.Time[i]);
            TimeGrouping group = time.GetTimeGrouping();

            pm10.Aggregators[group].AddValue(data.Hourly.Pm10[i]);
            pm2pt5.Aggregators[group].AddValue(data.Hourly.Pm2pt5[i]);
        }

        var da = new DailyPMAverages
        {
            Pm10 = new Averages
            {
                Morning = pm10.Aggregators[TimeGrouping.Morning_06to12].GetAverage(),
                Afternoon = pm10.Aggregators[TimeGrouping.Afternoon_12to18].GetAverage(),
                Night = pm10.Aggregators[TimeGrouping.Night_18to06].GetAverage(),
            },
            Pm2pt5 = new Averages
            {
                Morning = pm2pt5.Aggregators[TimeGrouping.Morning_06to12].GetAverage(),
                Afternoon = pm2pt5.Aggregators[TimeGrouping.Afternoon_12to18].GetAverage(),
                Night = pm2pt5.Aggregators[TimeGrouping.Night_18to06].GetAverage(),
            }
        };

        return da;
    }
}

internal class TimeSpanAggregator
{
    public Dictionary<TimeGrouping, MeasurmentAggregator> Aggregators { get; init; } =
        Enum.GetValues<TimeGrouping>().ToDictionary(e => e, e => new MeasurmentAggregator());
}

internal class MeasurmentAggregator
{
    public int HourCount { get; set; } = 0;
    public double AggregatedValue { get; set; } = 0;

    public void AddValue(double value)
    {
        HourCount++;
        AggregatedValue += value;
    }

    public double GetAverage()
    {
        return AggregatedValue / HourCount;
    }
}