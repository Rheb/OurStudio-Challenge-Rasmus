using Core.Models;
using System.Runtime.CompilerServices;

namespace Core.Logic;



public class AveragesLogic
{
    /// <summary>
    /// Keep for reference
    /// </summary>
    public DailyPMAverages Defunct_ParsePollutionData(HourlyData data)
    {
        double morningPm10 = 0, morningPm25 = 0;
        double afternoonPm10 = 0, afternoonPm25 = 0;
        double nightPm10 = 0, nightPm25 = 0;

        for (int i = 0; i < data.Hourly.Time.Length; i++)
        {
            var time = DateTime.Parse(data.Hourly.Time[i]);

            if (time.Hour > 6 && time.Hour < 12)
            {
                morningPm10 += data.Hourly.Pm10[i];
                morningPm25 += data.Hourly.Pm2pt5[i];
            }
            else if (time.Hour > 12 && time.Hour < 18)
            {
                afternoonPm10 += data.Hourly.Pm10[i];
                afternoonPm25 += data.Hourly.Pm2pt5[i];
            }
            else if (time.Hour < 6 || time.Hour > 18)
            {
                nightPm10 += data.Hourly.Pm10[i];
                nightPm25 += data.Hourly.Pm2pt5[i];
            }
        }

        var da = new DailyPMAverages
        {
            Pm10 = new Averages
            {
                Morning = morningPm10 / 6,
                Afternoon = afternoonPm10 / 6,
                Night = nightPm10 / 12
            },
            Pm2pt5 = new Averages
            {
                Morning = morningPm25 / 6,
                Afternoon = afternoonPm25 / 6,
                Night = nightPm25 / 12
            }
        };

        return da;
    }

    public DailyPMAverages GetAvgPollutionValues(HourlyData data)
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