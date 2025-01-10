namespace DeliveryConsoleApp;

class Program
{
    static void Main()
    {
        DataSample dataSample = new DataSample();
        UserInterface userInterface = new UserInterface();

        var shp = dataSample.CreateRandomShipment();
        userInterface.MainMenu(shp);
    }
}