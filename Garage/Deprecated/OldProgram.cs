using Garage.Entities;

namespace Garage.Deprecated;

public static class OldProgram
{
    private static IUI UI = ConsoleUI.Instance;

    static void OldMain(string[] args)
    {
#if NOTNOW
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfigurationRoot configuration = builder.Build();

        var configurationReader = new ConfigurationReader(configuration);
        var vehicleTypes = configurationReader.LoadSection("VehicleTypes");
#endif

        ConsoleMenu mainMenu = new ConsoleMenu("Welcome to the garage!", null, true);
        OldGarageHandler handler = new OldGarageHandler();

        mainMenu.AddItem("1. List all parked vehicles", () =>
        {
            foreach (IVehicle vehicle in handler.GetVehicles())
                handler.ListAllParkedVehicles();
        })
        .AddItem("2. List type of vehicles and amount", () => handler.ListVehicleTypes())
        .AddItem("3. Park a vehicle", () =>
        {
            handler.ParkCar();
        })
        .AddItem("4. Remove a vehicle", () =>
        {
            UI.Write("Please, specify the registration number of the vehicle to be removed:");
            string registrationNumber = UI.Read();
            handler.RemoveVehicle(registrationNumber);
        })
        .AddItem("5. Search vehicle by registration number", () =>
        {
            handler.FindVehicleByRegNumber();
        })
        .AddItem("6. Search-criteria based search", () =>
        {
            handler.SearchVehiclesByProperties();
        })
        .AddItem("7. Change the garage's capacity", () =>
        {
            handler.SetGarageCapacity();
        })
        .AddItem("8. Exit", () =>
        {
            UI.Write("Thank you for visiting!");
            UI.Display();
            Environment.Exit(0);
        });

        mainMenu.Run();

        bool runForever = true;

        while (runForever) { }
    }
}
