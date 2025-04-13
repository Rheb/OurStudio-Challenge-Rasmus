using Core.Logic;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using PollutionApi.Models;
using System.Linq.Expressions;

namespace PollutionApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PollutionReader(HttpClient client) : ControllerBase
    {
        private HttpClient HttpClient { get; init; } = client;

        [HttpGet]
        public async Task<DailyPMAverageResponse> GetDefaultPollutionData()
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

            return await FetchData(defaultRequest);
        }

        [HttpGet]
        public async Task<DailyPMAverageResponse> GetPollutionData([FromQuery] PollutionApiRequest request)
        {
            return await FetchData(request);
        }

        private async Task<DailyPMAverageResponse> FetchData(PollutionApiRequest request)
        {
            string errorMessage = "";

            try
            {
                string url = request.GetUrl();

                HttpResponseMessage res = await HttpClient.GetAsync(url);
                HourlyData? data = await res.Content.ReadFromJsonAsync<HourlyData>();

                if (
                    data != null
                    && data.Hourly.Time.Length > 0
                )
                {
                    DailyPMAverages averages = AveragesLogic.GetAvgPMValues(data);

                    return new DailyPMAverageResponse
                    {
                        Pm10 = averages.Pm10,
                        Pm2pt5 = averages.Pm2pt5,
                        Summary = averages.GetSummary(),
                    };
                }
                else
                {
                    errorMessage = "Data is null or empty";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An unknown error has occured {ex}";
            }

            return new DailyPMAverageResponse
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
