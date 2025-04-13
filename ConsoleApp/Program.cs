using Core.Logic;
using Core.Models;
using System.Text.Json;

try
{
    var bytes = await File.ReadAllBytesAsync("database/pollution_data.json");

    HourlyData? data = JsonSerializer.Deserialize<HourlyData?>(bytes);

    if (data != null)
    {
        DailyPMAverages defunct_da = AveragesLogic.Defunct_ParsePollutionData(data);

        Console.WriteLine("PM-10 averages:");
        Console.WriteLine($"\tMorning: {defunct_da.Pm10.Morning:0.00} μg/m³");
        Console.WriteLine($"\tAfternoon: {defunct_da.Pm10.Afternoon:0.00} μg/m³");
        Console.WriteLine($"\tNight: {defunct_da.Pm10.Night:0.00} μg/m³");

        Console.WriteLine("PM-2.5 averages:");
        Console.WriteLine($"\tMorning: {defunct_da.Pm2pt5.Morning:0.00} μg/m³");
        Console.WriteLine($"\tAfternoon: {defunct_da.Pm2pt5.Afternoon:0.00} μg/m³");
        Console.WriteLine($"\tNight: {defunct_da.Pm2pt5.Night:0.00} μg/m³");

        DailyPMAverages averages = AveragesLogic.GetAvgPollutionValues(data);

        Console.WriteLine();
        Console.WriteLine(averages.GetSummary());
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}