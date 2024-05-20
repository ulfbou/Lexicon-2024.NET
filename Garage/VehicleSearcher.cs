using Garage.Entities;

namespace Garage;

/// <summary>
/// Represents a searcher for vehicles based on specified properties.
/// </summary>
public class VehicleSearcher
{
    private readonly PropertySelector propertySelector;
    private readonly IEnumerable<IVehicle> garage;
    private readonly IUI ui;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleSearcher"/> class with the specified dependencies.
    /// </summary>
    /// <param name="propertySelector">The property selector used for specifying search criteria.</param>
    /// <param name="garage">The collection of vehicles to search within.</param>
    /// <param name="ui">The user interface to interact with.</param>
    public VehicleSearcher(
        PropertySelector propertySelector,
        IEnumerable<IVehicle> garage,
        IUI ui)
    {
        this.propertySelector = propertySelector;
        this.garage = garage;
        this.ui = ui;
    }

    /// <summary>
    /// Starts the process of searching vehicles by properties.
    /// </summary>
    public void SearchVehiclesByProperties()
    {
        SimpleMenu menu = new SimpleMenu("Advanced vehicle search", false);

        // Add vehicle type selection
        VehicleManager vehicleManager = new VehicleManager();
        SimpleMenu vehicleMenu = CreateVehicleMenu(vehicleManager, menu);
        menu.AddSubMenu(vehicleMenu.Title, vehicleMenu);

        // Add property selection
        menu.AddMenuItem("Select the brand",
            () => SelectProperty(VehicleManager.PROP_BRAND),
            false);
        menu.AddMenuItem("Select color", () => SelectProperty(VehicleManager.PROP_COLOR), false);
        menu.AddMenuItem("Number of wheels", () => SelectProperty(VehicleManager.PROP_WHEELS), false);
        menu.AddMenuItem("Special property", () => SelectProperty(VehicleManager.PROP_SPEC), false);

        // Add search functionality
        menu.AddMenuItem("Search by selected criterion", () => PerformSearch(), false);

        menu.Run();
    }

    private SimpleMenu CreateVehicleMenu(VehicleManager vehicleManager, SimpleMenu parent)
    {
        SimpleMenu vehicleMenu = new SimpleMenu("Select type of vehicle");

        foreach (string vehicleType in vehicleManager.GetVehicleTypes())
        {
            vehicleMenu.AddMenuItem(vehicleType, () =>
            {
                propertySelector[VehicleManager.PROP_TYPE] = vehicleType;
                ui.Write($"Selected vehicle type: {vehicleType}");
            }, true);
        }

        return vehicleMenu;
    }

    private void SelectProperty(string propertyName)
    {
        ui.Write($"What is the {propertyName.ToLower()} of the vehicle?");
        string propertyValue = ui.Read();
        propertySelector[propertyName] = propertyValue;
        ui.Write($"Selected {propertyName.ToLower()}: {propertyValue}");
    }

    private void PerformSearch()
    {
        foreach (IVehicle vehicle in garage)
        {
            bool match = true;

            foreach (var kvp in propertySelector)
            {
                switch (kvp.Key)
                {
                    case VehicleManager.PROP_TYPE:
                        if (vehicle.VehicleType != kvp.Value)
                        {
                            match = false;
                        }
                        break;
                    case VehicleManager.PROP_BRAND:
                        if (vehicle.Brand != kvp.Value)
                        {
                            match = false;
                        }
                        break;
                    case VehicleManager.PROP_COLOR:
                        if (vehicle.Color != kvp.Value)
                        {
                            match = false;
                        }
                        break;
                    case VehicleManager.PROP_WHEELS:
                        if (!int.TryParse(kvp.Value, out int wheels) || vehicle.NumberOfWheels != wheels)
                        {
                            match = false;
                        }
                        break;
                }
            }

            if (match)
            {
                ui.Write(vehicle.ToString());
            }
        }
    }
}
