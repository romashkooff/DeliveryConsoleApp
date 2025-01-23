using System.Text.Json;

namespace DeliveryConsoleApp;

public class Convertor
{
    
    public string GoodsStatisticsSerialized(List<ItemsGrouped> data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        
        return JsonSerializer.Serialize(data, options);
    }

    public List<ItemsGrouped> GoodsStatisticsDeserialized(string json)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        
        return JsonSerializer.Deserialize<List<ItemsGrouped>>(json, options) ?? throw new InvalidOperationException();
    }

    public async Task SaveToFile(string fileName, string data)
    { 
        await File.WriteAllTextAsync(fileName, data)
            .ContinueWith(task =>
            {
                    Console.WriteLine($"Saving to the file failed: {task.Exception.GetBaseException().Message}");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
    }

    public async Task<string> LoadFromFile(string fileName)
    {
        return await File.ReadAllTextAsync(fileName)
            .ContinueWith(task =>
            {
                    Console.WriteLine($"Loading from the file failed: {task.Exception.GetBaseException().Message}");
                    return String.Empty;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
    }
}