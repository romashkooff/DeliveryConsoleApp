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

    public List<(int orderNum, decimal deliveryPrice)> PriceCalculation(Transportation shp)
    {
        return StopPrice(shp);
    }

    public decimal TotalPriceCalculation (Transportation shp)
    {
        var stopPrice = StopPrice(shp);
        
        return stopPrice
            .Sum(stop => stop.deliveryPrice);
    }

    public IEnumerable<(int orderNum, IEnumerable<(string Key, int DeliveredCount)>)> GoodsStatistics(Transportation shp)
    {
        return shp.Stops
            .Where(stop => stop.Deliveries != null)
            .Select(stop =>
            (
                stop.StopOrder,
                stop.Deliveries
                    .SelectMany(delivery => delivery.Items)
                    .GroupBy(item => item.Name)
                    .Select(itemsGrouped =>
                    (
                        itemsGrouped.Key,
                        itemsGrouped.Sum(item => item.DeliveredCount)
                    ))
            ));
    }

    public double TotalCarDistance(Transportation shp)
    {
        return Math.Round(shp.Stops
            .Sum(stop => stop.DistanceFromPreviousStop), 2);
    }

    public (string Name, int DeliveredCount) MostGoodsCustomer(Transportation shp)
    {
        return shp.Stops
            .Where(stop => stop.Deliveries != null)
            .Select(stop => 
            (
                stop.Customer.Name,
                stop.Deliveries
                    .SelectMany(delivery => delivery.Items)
                    .Select(item => item.DeliveredCount)
                    .Sum()
            ))
            .OrderByDescending(result => result.Item2)
            .FirstOrDefault();
    }

    public IEnumerable<(int orderNum, double distanceFromPreviousStop)> StopsByDistance(Transportation shp)
    {
        return shp.Stops
            .Where(stop => stop.Deliveries != null)
            .OrderByDescending(stop => stop.DistanceFromPreviousStop)
            .Select(stop =>
            (
                stop.StopOrder,
                Math.Round(stop.DistanceFromPreviousStop, 2)
            ));
    }

    public IEnumerable<(int orderNum, decimal deliveryPrice)> deliveriesByPrice(Transportation shp)
    {
        return StopPrice(shp)
            .OrderByDescending(stop => stop.deliveryPrice);
    }
}