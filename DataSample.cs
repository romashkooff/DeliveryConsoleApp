namespace DeliveryConsoleApp;
public class DataSample
{
    private static Random Random = new Random();

    private Dictionary<string, decimal> materials = new Dictionary<string, decimal>()
    {
        { "Beer Crate 1", 146.90m },
        { "Beer Crate 2", 286.30m },
        { "Birel Pomelo", 119.90m },
        { "PU Keg 50l", 2468.50m },
        { "GA KEG 30l", 1120 }
    };

    public Transportation CreateRandomShipment()
    {
        return new Transportation()
        {
            Id = Guid.NewGuid().ToString(),
            Truck = new Vehicle()
            {
                Id = "PL001",
                LicensePlate = "PLZEN001"
            },
            Stops = CreateStops()
        };
    }

    #region private methods

    private List<Stop> CreateStops()
    {
        List<Stop> stops = new List<Stop>();
        int count = Random.Next(3, 30);
        int order = 1;
        stops.Add(new Stop()
        {
            StopOrder = order++,
            Customer = CreateDepot()
        });

        for (; order < count; order++)
        {
            var stop = new Stop()
            {
                DistanceFromPreviousStop = (Random.NextDouble() + 10) * 100,
                StopOrder = order,
                Customer = CreateCustomer(),
                Deliveries = new List<Delivery>()
            };

            for (int i = 0; i < Random.Next(1, 4); i++)
            {
                stop.Deliveries.Add(CreateDelivery());
            }
            stops.Add(stop);
        }

        stops.Add(new Stop()
        {
            StopOrder = order,
            Customer = CreateDepot()
        });

        return stops;
    }

    private Customer CreateDepot()
    {
        return new Customer()
        {
            Id = "8CAA",
            Name = "DC Plzeň"
        };
    }

    private Customer CreateCustomer()
    {
        var id = Guid.NewGuid().ToString();
        return new Customer()
        {
            Id = id,
            Name = "Customer " + id,
        };
    }

    private Delivery CreateDelivery()
    {
        Delivery delivery = new Delivery()
        {
            Id = Guid.NewGuid().ToString(),
            Items = new List<DeliveryItem>()
        };

        for (int i = 0; i < Random.Next(1, 20); i++)
        {
            delivery.Items.Add(CreateDeliveryItem());
        }

        return delivery;
    }

    private DeliveryItem CreateDeliveryItem()
    {
        var cnt = Random.Next(1, 100);
        var mat = materials.ElementAt(Random.Next(0, materials.Count));
        return new DeliveryItem()
        {
            OrderedCount = cnt,
            DeliveredCount = Random.Next(1, cnt),
            Id = Guid.NewGuid().ToString(),
            Name = mat.Key,
            SalesUnitPrice = mat.Value,
        };
    }

    #endregion
}

#region Data Structures

public class Transportation
{
    public string Id { get; set; }
    public Vehicle Truck { get; set; }
    public List<Stop> Stops { get; set; }
}

public class Vehicle
{
    public string Id { get; set; }
    public string LicensePlate { get; set; }
}

public class Stop
{
    public int StopOrder { get; set; }
    public double DistanceFromPreviousStop { get; set; }

    public Customer Customer { get; set; }

    public List<Delivery> Deliveries { get; set; }
}

public class Customer
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class Delivery
{
    public string Id { get; set; }
    public List<DeliveryItem> Items { get; set; }
}

public class DeliveryItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal SalesUnitPrice { get; set; }
    public int OrderedCount { get; set; }
    public int DeliveredCount { get; set; }
}

#endregion

