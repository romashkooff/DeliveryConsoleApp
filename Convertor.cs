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
        var orders = JsonSerializer.Deserialize<List<Order>>(json, options)
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
}

#region Data Structures

public class Order
{
    public int OrderNum { get; set; }
    public List<Item> ItemsList { get; set; }
}

public class Item
{
    public string Name { get; set; }
    public int DeliveredCount { get; set; }
}

#endregion