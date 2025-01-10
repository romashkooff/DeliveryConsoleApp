using Spectre.Console;

namespace DeliveryConsoleApp;

internal class Calculations
{
    private List<(int orderNum, decimal deliveryPrice)> StopPrice(Transportation shp)
    {
        var stopPrice = shp.Stops
            .Where(stop => stop.Deliveries != null)
            .Select(stop =>
            (
                stop.StopOrder,
                stop.Deliveries
                    .SelectMany(delivery => delivery.Items)
                    .Sum(item => item.SalesUnitPrice * item.DeliveredCount)
            ))
            .ToList();
        return stopPrice;
    }

    public void PriceCalculation(Transportation shp)
    {
        var stopPrice = StopPrice(shp);

        var totalDeliveryPrice = stopPrice
            .Sum(stop => stop.deliveryPrice);

        foreach (var stop in stopPrice)
        {
            Console.WriteLine($"Stop #{stop.orderNum}. Delivery price is: {stop.deliveryPrice} koruns.");
        }
        Console.WriteLine($"\nTotal delivery price is: {totalDeliveryPrice}.\n");

        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    public void GoodsStatistics(Transportation shp)
    {
        var stopItemStatistics = shp.Stops
            .Where (stop => stop.Deliveries != null)
            .Select(stop => new
            {
                stopNum = stop.StopOrder,
                stopItems = stop.Deliveries
                .SelectMany(delivery => delivery.Items)
                .GroupBy(item => item.Name)
                .Select(itemsGrouped => new
                {
                    itemName = itemsGrouped.Key,
                    itemCount = itemsGrouped.Sum(item => item.DeliveredCount)
                })
            })
            .ToList();

        foreach (var stop in stopItemStatistics)
        {
            Console.WriteLine($"\nStop #{stop.stopNum}:\n");
            foreach (var item in stop.stopItems)
            {
                Console.WriteLine($"Item name: {item.itemName}");
                Console.WriteLine($"Item count: {item.itemCount}");
            }
        }

        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    public void CarStatistics(Transportation shp)
    {
        var totalCarDistance = Math.Round(shp.Stops
            .Sum(stop => stop.DistanceFromPreviousStop), 2)
            .ToString();

        Console.WriteLine($"Total car distance is: {totalCarDistance} km.\n");

        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    public void MostGoodsCustomer(Transportation shp)
    {
        var mostGoodsCustomer = shp.Stops
            .Where(stop => stop.Deliveries != null)
            .Select(stop => new
            {
                CustomerName = stop.Customer.Name,
                itemsCount = stop.Deliveries
                    .SelectMany(delivery => delivery.Items)
                    .Select(item => item.DeliveredCount)
                    .Sum()
            })
            .OrderByDescending(result => result.itemsCount)
            .First();

        Console.WriteLine($"Most frequent customer is: {mostGoodsCustomer.CustomerName}\n");

        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    public void StopsByDistance(Transportation shp)
    {
        var stopsByDistance = shp.Stops
            .OrderByDescending(stop => stop.DistanceFromPreviousStop)
            .Select(stop => new
            {
                stopNum = stop.StopOrder,
                stopDistance = Math.Round(stop.DistanceFromPreviousStop, 2)
            })
            .ToList();

        foreach (var stop in stopsByDistance)
        {
            Console.WriteLine($"Stop #{stop.stopNum} with the distance of {stop.stopDistance} km.");
        }

        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    public void ShipmentByPrice(Transportation shp)
    {
        var deliveriesByTotalPrice = StopPrice(shp)
            .OrderByDescending(stop => stop.deliveryPrice)
            .ToList();

        foreach (var stop in deliveriesByTotalPrice)
        {
            Console.WriteLine($"Stop #{stop.orderNum} with the distance of {stop.deliveryPrice} koruns.");
        }

        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }
}