using Spectre.Console;
using static DeliveryConsoleApp.Enums;

namespace DeliveryConsoleApp;

internal class UserInterface
{
    private readonly Calculations _shpCalc = new Calculations();
    
    internal void MainMenu(Transportation shp)
    {
        while (true)
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuAction>()
                .Title("Choose the option:")
                .AddChoices(Enum.GetValues<MenuAction>()));

            if (choice == MenuAction.Exit)
            {
                return;
            }

            switch (choice)
            {
                case MenuAction.PriceCalculation:
                    _shpCalc.PriceCalculation(shp);
                    break;
                case MenuAction.GoodsStatistics:
                    _shpCalc.GoodsStatistics(shp);
                    break;
                case MenuAction.CarStatistics:
                    _shpCalc.CarStatistics(shp);
                    break;
                case MenuAction.MostGoodsCustomer:
                    _shpCalc.MostGoodsCustomer(shp);
                    break;
                case MenuAction.StopsByDistance:
                    _shpCalc.StopsByDistance(shp);
                    break;
                case MenuAction.ShipmentByPrice:
                    _shpCalc.ShipmentByPrice(shp);
                    break;
                case MenuAction.Exit:
                    break;
            }
        }
    }
}

