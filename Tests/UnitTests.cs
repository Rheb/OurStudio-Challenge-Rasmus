using Core.Logic;
using Core.Models;

namespace Tests
{
    public class UnitTests
    {
        [Fact]
        public void Test_TimeGrouping()
        {
            DateTime now = DateTime.Now.Date;

            Assert.True(now.GetTimeGrouping().Equals(TimeGrouping.Night_18to06));
            Assert.True(now.AddHours(6).AddTicks(-1).GetTimeGrouping().Equals(TimeGrouping.Night_18to06));

            Assert.True(now.AddHours(6).GetTimeGrouping().Equals(TimeGrouping.Morning_06to12));
            Assert.True(now.AddHours(12).AddTicks(-1).GetTimeGrouping().Equals(TimeGrouping.Morning_06to12));

            Assert.True(now.AddHours(12).GetTimeGrouping().Equals(TimeGrouping.Afternoon_12to18));
            Assert.True(now.AddHours(18).AddTicks(-1).GetTimeGrouping().Equals(TimeGrouping.Afternoon_12to18));

            Assert.True(now.AddHours(18).GetTimeGrouping().Equals(TimeGrouping.Night_18to06));
            Assert.True(now.AddHours(18 + 12).AddTicks(-1).GetTimeGrouping().Equals(TimeGrouping.Night_18to06));
        }

        [Fact]
        public void Test_Averages()
        {
            HourlyData hourly = new();

            List<int> nums = [];

            double mBase = 0, aBase = 0, nBase = 0;

            for (int i = 0; i < 24; i++)
            {
                nums.Add(i);

                if (i < 6)
                {
                    nBase += i;
                }
                else if (i < 12)
                {
                    mBase += i;
                }
                else if (i < 18)
                {
                    aBase += i;
                }
                else // >= 18
                {
                    nBase += i;
                }
            }

            hourly.Hourly.Time = [.. nums.Select(i => $"{DateTime.Now.Date.AddHours(i):yyyy-MM-dd HH:mm}")];
            hourly.Hourly.Pm2pt5 = [.. nums.Select(i => i * 2.0)];
            hourly.Hourly.Pm10 = [.. nums.Select(i => i * 17.0)];

            var avgs = AveragesLogic.GetAvgPMValues(hourly);

            Assert.True(avgs.Pm2pt5.Morning == mBase * 2 / 6);
            Assert.True(avgs.Pm2pt5.Afternoon == aBase * 2 / 6);
            Assert.True(avgs.Pm2pt5.Night == nBase * 2 / 12);

            Assert.True(avgs.Pm10.Morning == mBase * 17 / 6);
            Assert.True(avgs.Pm10.Afternoon == aBase * 17 / 6);
            Assert.True(avgs.Pm10.Night == nBase * 17 / 12);
        }
    }
}