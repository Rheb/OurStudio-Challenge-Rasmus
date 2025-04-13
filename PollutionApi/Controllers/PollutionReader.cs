using Core.Logic;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using PollutionApi.Models;

namespace PollutionApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PollutionReader(HttpClient client) : ControllerBase
    {
        private HttpClient HttpClient { get; init; } = client;

        [HttpGet(Name = "GetDefaultPollutionData")]
        public async Task<DailyPMAverages> GetDefaultPollutionData()
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);

            PollutionApiRequest defaultRequest = new()
            {
                Latitude = 52.5235,
                Longitude = 13.4115,
                MeasurmentTypes = ["pm10", "pm2_5"],
                StartDate = yesterday,
                EndDate_Inclusive = yesterday,
            };

            DailyPMAverages data = await FetchData(defaultRequest);

            return data;
        }

        [HttpGet(Name = "GetPollutionData")]
        public async Task<DailyPMAverages> GetPollutionData(PollutionApiRequest request)
        {
            DailyPMAverages data = await FetchData(request);

            return data;
        }

        private async Task<DailyPMAverages> FetchData(PollutionApiRequest request)
        {
            string url = request.GetUrl();

            HttpResponseMessage res = await HttpClient.GetAsync(url);
            HourlyData? data = await res.Content.ReadFromJsonAsync<HourlyData>();

            if (data is null)
            {
                // TODO Error handling
                return new DailyPMAverages();
            }

            DailyPMAverages averages = AveragesLogic.GetAvgPollutionValues(data);

            return averages;
        }
    }
}
