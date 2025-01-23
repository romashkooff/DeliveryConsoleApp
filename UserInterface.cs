using Spectre.Console;
using static DeliveryConsoleApp.Enums;
using static DeliveryConsoleApp.Enums.MenuAction;

namespace DeliveryConsoleApp;

public class UserInterface
{
    private readonly Calculations _shpCalc = new();
    private readonly Convertor _textConv = new();

    public void MainMenu(Transportation shp)
    {
        while (true)
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuAction>()
                    .Title("Choose the option:")
                    .AddChoices(Enum.GetValues<MenuAction>()));

            if (choice == Exit)
            {
                return;
            }

            switch (choice)
            {
                case PriceCalculation:
                    PriceCalculationInterpreted(shp);
                    TotalPriceCalculationInterpreted(shp);
                    PressAnyKeyToContinue();
                    break;
                case GoodsStatistics:
                    GoodsStatisticsInterpreted(shp);
                    PressAnyKeyToContinue();
                    break;
                case GoodsStatisticsSerialized:
                    GoodsStatisticsSerializedInterpreted(shp);
                    PressAnyKeyToContinue();
                    break;
                case GoodsStatisticsDeserialized:
                    GoodsStatisticsDeserializedInterpreted(shp);
                    PressAnyKeyToContinue();
                    break;
                case GoodsStatisticsSaveToFile:
                    _ = GoodsStatisticsSaveToFileInterpreted(shp);
                    PressAnyKeyToContinue();
                    break;
                case GoodsStatisticsLoadFromFile:
                    _ = GoodsStatisticsLoadFromFileInterpreted();
                    PressAnyKeyToContinue();
                    break;
                case CarStatistics:
                    CarStatisticsInterpreted(shp);
                    PressAnyKeyToContinue();
                    break;
                case MostGoodsCustomer:
                    MostGoodsCustomerInterpreted(shp);
                    PressAnyKeyToContinue();
                    break;
                case StopsByDistance:
                    StopsByDistanceInterpreted(shp);
                    PressAnyKeyToContinue();
                    break;
                case ShipmentByPrice:
                    ShipmentByPriceInterpreted(shp);
                    PressAnyKeyToContinue();
                    break;
                case Exit:
                    break;
            }
        }
    }

    private void PressAnyKeyToContinue()
    {
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }
    
    private void PriceCalculationInterpreted(Transportation shp)
    {
        var priceCalculation = _shpCalc.PriceCalculation(shp);

        foreach (var stop in priceCalculation)
        {
            Console.WriteLine($"Stop #{stop.orderNum}. Delivery price is: {stop.deliveryPrice} koruns.");
        }
    }

    private void TotalPriceCalculationInterpreted(Transportation shp)
    {
        var totalPriceCalculation = _shpCalc.TotalPriceCalculation(shp);

        Console.WriteLine($"\nTotal delivery price is: {totalPriceCalculation}.\n");
    }

    private void GoodsStatisticsInterpreted(Transportation shp)
    {
        var stopItemStatistics = _shpCalc.GoodsStatistics(shp);

        foreach (var stop in stopItemStatistics)
        {
            Console.WriteLine($"\nStop #{stop.orderNum}:\n");
            foreach (var item in stop.deliveryItems)
            {
                Console.WriteLine($"Item name: {item.Name}");
                Console.WriteLine($"Item count: {item.DeliveredCount}");
            }
        }
    }
    
    private void GoodsStatisticsSerializedInterpreted(Transportation shp)
    {
        var goodsStatistics = _shpCalc.GoodsStatistics(shp);
        
        var serialized = _textConv.GoodsStatisticsSerialized(goodsStatistics);
        
        Console.WriteLine(serialized);
    }

    private void GoodsStatisticsDeserializedInterpreted(Transportation shp)
    {
        var goodsStatistics = _shpCalc.GoodsStatistics(shp);
        var serialized = _textConv.GoodsStatisticsSerialized(goodsStatistics);
        var deserialized = _textConv.GoodsStatisticsDeserialized(serialized);
        
        foreach (var stop in deserialized)
        {
            Console.WriteLine($"\nStop #{stop.orderNum}:\n");
            foreach (var item in stop.deliveryItems)
            {
                Console.WriteLine($"Item name: {item.Name}");
                Console.WriteLine($"Item count: {item.DeliveredCount}");
            }
        }
    }

    private async Task GoodsStatisticsSaveToFileInterpreted(Transportation shp)
    {
        var goodsStatistics = _shpCalc.GoodsStatistics(shp);
        var serialized = _textConv.GoodsStatisticsSerialized(goodsStatistics);
        var fileName = "GoodsStatisticsSerialized.json";
        
        var saveTask = _textConv.SaveToFile(fileName, serialized);
        await saveTask;
        
        // Using async / await:
        // await _textConv.SaveToFile(fileName, serialized);
    }
    
    private async Task GoodsStatisticsLoadFromFileInterpreted()
    {
        var fileName = "GoodsStatisticsSerialized.json";
        
        var data = await _textConv.LoadFromFile(fileName);
        Console.WriteLine(data);
    }

    private void CarStatisticsInterpreted(Transportation shp)
    {
        var totalCarDistance = _shpCalc.TotalCarDistance(shp);

        Console.WriteLine($"Total car distance is: {totalCarDistance} km.\n");
    }

    private void MostGoodsCustomerInterpreted(Transportation shp)
    {
        var mostGoodsCustomer = _shpCalc.MostGoodsCustomer(shp);

        Console.WriteLine($"Most frequent customer is: {mostGoodsCustomer.Name}\n");
    }

    private void StopsByDistanceInterpreted(Transportation shp)
    {
        var stopsByDistance = _shpCalc.StopsByDistance(shp);
        
        foreach (var stop in stopsByDistance)
        {
            Console.WriteLine($"Stop #{stop.orderNum} with the distance of {stop.distance} km.");
        }
    }

    private void ShipmentByPriceInterpreted(Transportation shp)
    {
        var deliveriesByPrice = _shpCalc.DeliveriesByPrice(shp);
        
        foreach (var stop in deliveriesByPrice)
        {
            Console.WriteLine($"Stop #{stop.orderNum} with the distance of {stop.deliveryPrice} koruns.");
        }
    }
}

