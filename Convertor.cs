using System.Text.Json;

namespace DeliveryConsoleApp;

public class Convertor
{
    
    public string GoodsStatisticsSerialized(IEnumerable<(int orderNum, IEnumerable<(string Name, int DeliveredCount)>)> data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        List<Order> orders = data
            .Select(order => new Order
                {
                OrderNum = order.orderNum,
                ItemsList = order.Item2
                    .Select(item => new Item
                    {
                        Name = item.Name,
                        DeliveredCount = item.DeliveredCount
                    })
                    .ToList()
                })
                .ToList();
    return JsonSerializer.Serialize(orders, options);
    }

    public IEnumerable<(int orderNum, IEnumerable<(string Name, int DeliveredCount)>)> GoodsStatisticsDeserialized(string json)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var orders = (JsonSerializer.Deserialize<List<Order>>(json, options) ?? throw new InvalidOperationException())
            .Select(order =>
            (
                order.OrderNum,
                order.ItemsList
                    .Select(item =>
                    (
                        item.Name,
                        item.DeliveredCount
                    ))
            ));
        return orders;
    }

    public Task SaveToFile(string fileName, string data)
    {
        return File.WriteAllTextAsync(fileName, data)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine($"Saving to the file failed: {task.Exception.GetBaseException().Message}");
                }
                else
                {
                    Console.WriteLine($"Data is saved to the '{fileName}'");
                }
            });
    }

    public Task<string> LoadFromFile(string fileName)
    {
        return File.ReadAllTextAsync(fileName)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine($"Loading from the file failed: {task.Exception.GetBaseException().Message}");
                    return String.Empty;
                }
                else
                {
                    Console.WriteLine($"Data is loaded from the file: '{fileName}'");
                    return task.Result;
                }
            });
    }
}

#region Data Structures

public class Order
{
    public int OrderNum { get; init; }
    public required List<Item> ItemsList { get; init; }
}

public class Item
{
    public required string Name { get; init; }
    public int DeliveredCount { get; init; }
}

#endregion