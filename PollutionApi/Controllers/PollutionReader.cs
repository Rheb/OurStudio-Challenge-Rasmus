using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace PollutionApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PollutionReader : ControllerBase
    {
        private HttpClient HttpClient { get; init; }

        public PollutionReader(HttpClient client)
        {
            HttpClient = client;
        }

        [HttpGet(Name = "ReadPollutionData")]
        public async Task ReadPollutionData()
        {
            HttpResponseMessage res = await HttpClient.GetAsync("https://air-quality-api.open-meteo.com/v1/air-quality?latitude=52.5235&longitude=13.4115&hourly=pm10,pm2_5&start_date=2023-01-31&end_date=2023-01-31");
            string content = await res.Content.ReadAsStringAsync();
            HourlyData? data = await res.Content.ReadFromJsonAsync<HourlyData>();
        }
    }
}
