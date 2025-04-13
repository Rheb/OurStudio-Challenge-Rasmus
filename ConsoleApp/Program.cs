using Core.Logic;
using Core.Models;
using System.Text.Json;

try
{
    var bytes = await File.ReadAllBytesAsync("database/pollution_data.json");

    HourlyData? data = JsonSerializer.Deserialize<HourlyData?>(bytes);

    if (data != null)
    {
        DailyPMAverages averages = AveragesLogic.GetAvgPMValues(data);

        Console.WriteLine();
        Console.WriteLine(averages.GetSummary());
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}