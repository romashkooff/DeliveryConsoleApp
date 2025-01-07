using DeliveryConsoleApp;
using TestApp;

class Program
{
    static void Main(string[] args)
    {
        DataSample dataSample = new DataSample();
        UserInterface userInterface = new UserInterface();

        var shp = dataSample.CreateRandomShipment();
        userInterface.MainMenu(shp);
    }
}