namespace DeliveryConsoleApp;

public static class Extensions
{
    public static List<(int orderNum, decimal deliveryPrice)> StopPrice(this Transportation shp)
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
}