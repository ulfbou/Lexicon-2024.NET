
using Garage.Entities;
using System;
using System.Reflection;

namespace Garage;

public class GarageManager
{
    private readonly IUI ui = SimpleUI.Instance;
    private GarageHandler handler = null!;
    private VehicleManager vehicleManager = null!;
    private HashSet<string> registrationNumbers = new HashSet<string>();
    private int registrationCounter = 0;
    private Random random = new Random();

    public GarageManager(IUI ui)
    {
        Logger logger = Logger.Instance;
        logger.Log(LogLevel.Info, $"Initializing GarageManager.");
        this.ui = ui;
        this.handler = new GarageHandler(1);
        this.vehicleManager = new VehicleManager();
    }

    public void Run()
    {
        this.handler = new GarageHandler(4);
        //SetGarageCapacity();
        PopulateGarage();
        ListAllParkedVehicles();
        ui.Write("Press enter to continue, please.", ConsoleColor.White);
        ui.Read(true);

        Logger logger = Logger.Instance;
        logger.Log(LogLevel.Info, $"Starting GarageManager Main loop.");


        SimpleMenu mainMenu = new SimpleMenu("Welcome to the garage!", true);

        mainMenu
            .AddMenuItem("List all parked vehicles", () =>
            {
                ListAllParkedVehicles();
            })
            .AddMenuItem("List type of vehicles and amount", () =>
            {
                ListVehicleTypes();
            })
            .AddMenuItem("Park a vehicle", () =>
            {
                ParkVehicle();
            })
            .AddMenuItem("Remove a vehicle", () =>
            {
                RemoveVehicle();
            })
            .AddMenuItem("Search vehicle by registration number", () =>
            {
                SearchVehicleByRegNumber();
            })
            .AddMenuItem("Search-criteria based search", () =>
            {
                SearchVehiclesByProperties();
            })
            .AddMenuItem("Change the garage's capacity", () =>
            {
                SetGarageCapacity();
            })
            .AddMenuItem("Exit", () =>
            {
                ui.Write("Thank you for visiting!");
                Environment.Exit(0);
            });

        mainMenu.Run();
        logger.Log(LogLevel.Info, "Ending Garage Manager main loop.");
    }

    private void ListAllParkedVehicles()
    {
        IEnumerable<IVehicle> parkedVehicles = this.handler;
        int count = 0;

        ui.Write("Here is a list of all parked Vehicles.", ConsoleColor.White);
        foreach (IVehicle vehicle in parkedVehicles)
        {
            Vehicle v = vehicle as Vehicle;
            ui.Write($"{v}");
            count++;
        }

        if (count == 0)
        {
            ui.Write("There are currently no vehicles parked at the moment.");
        }
    }

    private void ListVehicleTypes()
    {
        Dictionary<string, int> vehicleTypeCounter = new Dictionary<string, int>();

        foreach (IVehicle vehicle in handler)
        {
            if (!vehicleTypeCounter.ContainsKey(vehicle.VehicleType))
            {
                vehicleTypeCounter[vehicle.VehicleType] = 1;
            }
            else
            {
                vehicleTypeCounter[vehicle.VehicleType]++;
            }
        }

        ui.Write($"Here is a list of the types of vehicle and how many of each has been parked.");

        foreach (KeyValuePair<string, int> kvp in vehicleTypeCounter)
        {
            ui.Write($"{kvp.Key} {kvp.Value}");
        }
    }

    // TODO: Review parkvehicle
    public void ParkVehicle()
    {
        if (handler.Count == handler.Capacity)
        {
            ui.Write("The garage is full at the moment. Please, unpark another car before you try this again.");
            return;
        }

        PropertySelector propertySelector = new PropertySelector();
        SimpleMenu vehicleMenu = CreateVehicleMenu(propertySelector);
        vehicleMenu.Run(ConsoleKey.Enter);

        string selectedType = propertySelector[VehicleManager.PROP_TYPE];

        if (string.IsNullOrEmpty(selectedType))
        {
            ui.Write("Please select a valid vehicle type.");
            return;
        }

        ParameterInfo[] parameters = vehicleManager.GetConstructorParameters(selectedType).ToArray();
        if (parameters == null || parameters.Length == 0)
        {
            ui.Write("Selected vehicle type has no properties.");
            return;
        }

        foreach (ParameterInfo parameter in parameters)
        {
            ui.Write($"What is the {parameter.Name} of the vehicle?");
            string propertyValue = ui.Read();
            propertySelector[parameter.Name] = propertyValue;
        }


        // TODO: Merge code with createManager
        try
        {
            Type? type = vehicleManager.GetVehicleType(selectedType);
            if (type == null)
            {
                ui.Write($"Could not find type: {selectedType}.");
                return;
            }

            object? obj = Activator.CreateInstance(type);
            if (obj is IVehicle vehicle)
            {
                // Set properties
                foreach (var parameter in parameters)
                {
                    PropertyInfo propInfo = type.GetProperty(parameter.Name);
                    if (propInfo != null)
                    {
                        object value = Convert.ChangeType(propertySelector[parameter.Name], propInfo.PropertyType);
                        propInfo.SetValue(vehicle, value);
                    }
                }

                // Add vehicle to garage
                handler.Add(vehicle);
                ui.Write($"A {vehicle.VehicleType} has now been parked.");
            }
            else
            {
                ui.Write($"Could not create a {selectedType}.");
            }
        }
        catch (Exception ex)
        {
            ui.Write($"Error creating instance: {ex.Message}");
        }
    }

    public void ParkVehicle(PropertySelector propertySelector)
    {
        // Get the selected vehicle type from the property selector
        string selectedType = propertySelector[VehicleManager.PROP_TYPE];

        // Create the vehicle using the selected type and property selector
        IVehicle vehicle = vehicleManager.CreateVehicle(selectedType, propertySelector);

        if (vehicle != null)
        {
            handler.Add(vehicle);
            ui.Write($"A {vehicle.VehicleType} has now been parked.");
        }
        else
        {
            ui.Write($"Could not create a {selectedType}.");
        }
    }

    private SimpleMenu CreateVehicleMenu(
        PropertySelector propertySelector, 
        bool confirm = true)
    {
        SimpleMenu vehicleMenu = new SimpleMenu("Select type of vehicle");

        // Get the vehicle types from the VehicleManager
        IEnumerable<string> vehicleTypes = vehicleManager.GetVehicleTypes();

        foreach (string vehicleType in vehicleTypes)
        {
            vehicleMenu.AddMenuItem(vehicleType, () =>
            {
                propertySelector[VehicleManager.PROP_TYPE] = vehicleType;
                ui.Write($"Selected vehicle type: {vehicleType}");
            }, false);
        }

        return vehicleMenu;
    }


    /*
    public void ParkVehicle3()
    {
        if (handler.Count == handler.Capacity)
        {
            ui.Write("The garage is full at the moment. Please, unpark another car before you try this again.");
            return;
        }

        PropertySelector propertySelector = new PropertySelector();
        SimpleMenu vehicleMenu = CreateVehicleMenu(propertySelector);
        vehicleMenu.Run(ConsoleKey.Enter);

        string selectedType = propertySelector[VehicleManager.PROP_TYPE];

        if (string.IsNullOrEmpty(selectedType))
        {
            ui.Write("Please select a valid vehicle type.");
            return;
        }

        var properties = vehicleManager.GetVehicleProperties(selectedType);

        if (properties == null || properties.Count == 0)
        {
            ui.Write("Selected vehicle type has no properties.");
            return;
        }

        foreach (IProperty property in properties)
        {
            ui.Write($"What is the {property.Name} of the vehicle?");
            string propertyValue = ui.Read();
            propertySelector[property.Name] = propertyValue;
        }

        try
        {
            Type? type = vehicleManager.GetVehicleType(selectedType);
            if (type == null)
            {
                ui.Write($"Could not find type: {selectedType}.");
                return;
            }

            object? obj = Activator.CreateInstance(type);
            if (obj is IVehicle vehicle)
            {
                // Set properties
                foreach (var property in properties)
                {
                    PropertyInfo propInfo = type.GetProperty(property.Key);
                    if (propInfo != null)
                    {
                        object value = Convert.ChangeType(propertySelector[property.Key], propInfo.);
                        propInfo.SetValue(vehicle, value);
                    }
                }

                // Add vehicle to garage
                handler.Add(vehicle);
                ui.Write($"A {vehicle.VehicleType} has now been parked.");
            }
            else
            {
                ui.Write($"Could not create a {selectedType}.");
            }
        }
        catch (Exception ex)
        {
            ui.Write($"Error creating instance: {ex.Message}");
        }
    }

    private SimpleMenu CreateVehicleMenu(PropertySelector propertySelector)
    {
        SimpleMenu vehicleMenu = new SimpleMenu("Select type of vehicle");

        foreach (string vehicleType in vehicleManager.GetVehicleTypes())
        {
            vehicleMenu.AddMenuItem(vehicleType, () =>
            {
                propertySelector[VehicleManager.PROP_TYPE] = vehicleType;
            }, false);
        }

        return vehicleMenu;
    }


    private void ParkVehicle2()
    {
        if (handler.Count == handler.Capacity)
        {
            ui.Write("The garage is full at the moment. Please, unpark another car before you try this again.");
            return;
        }

        PropertySelector propertySelector = new PropertySelector();
        SimpleMenu vehicleMenu = CreateVehicleMenu2(propertySelector, null, false);
        vehicleMenu.Run(ConsoleKey.Enter);

        string selectedType = propertySelector[VehicleManager.PROP_TYPE];
        //ui.Write(selectedType, ConsoleColor.Green);

        string registrationNumber = GetRegNum();

        PropertyQuery(selectedType, VehicleManager.PROP_BRAND, propertySelector);
        PropertyQuery(selectedType, VehicleManager.PROP_COLOR, propertySelector);
        PropertyQuery(selectedType, VehicleManager.PROP_SPEC, propertySelector);

        try
        {
            Type? type = vehicleManager.GetVehicleType((string)selectedType);
            if (type is null)
            {
                type = Type.GetType(selectedType);
            }

            if (type is null)
            {
                ui.Write($"1. Could not create a {selectedType}.");
                return;
            }

            string brand = propertySelector[VehicleManager.PROP_BRAND];
            string color = propertySelector[VehicleManager.PROP_COLOR];
            string specificArg = propertySelector[VehicleManager.PROP_SPEC];

            object? obj = Activator.CreateInstance(type, registrationNumber, brand, color, specificArg);

            if (obj is not null && obj is IVehicle vehicle)
            {
                handler.Add(vehicle);
                ui.Write($"A {vehicle.VehicleType} has now been parked.");
            }
            else
            {
                ui.Write($"Could not create a {selectedType}.");
            }
        }
        catch (Exception ex)
        {
            ui.Write($"Error creating instance: {ex.Message}");
        }
    }

    private SimpleMenu CreateVehicleMenu2(PropertySelector propertySelector, SimpleMenu parent, bool confirm = true)
    {
        SimpleMenu vehicleMenu = new SimpleMenu("Select type of vehicle");

        foreach (string vehicleType in vehicleManager.GetVehicleTypes())
        {
            vehicleMenu.AddMenuItem(vehicleType, () =>
            {

                propertySelector[VehicleManager.PROP_TYPE] = vehicleType;
                //ui.Write($"Selected vehicle type: {vehicleType}");
            }, false);
        }

        return vehicleMenu;
    }
    */

    private void RemoveVehicle()
    {
        ui.Write("Please, specify the registration number of the vehicle to be removed:");
        string registrationNumber = ui.Read();

        IVehicle? vehicle = handler.Remove(registrationNumber);

        if (vehicle is not null)
        {
            ui.Write($"{vehicle.VehicleType} with the registration number {registrationNumber} is no longer parking.");
        }
        else
        {
            ui.Write($"There is no vehicle parked by the registration number {registrationNumber}.");
        }
    }

    private void SearchVehicleByRegNumber()
    {
        ui.Write("Please, specify the registration number of the vehicle to be located.");
        string registrationNumber = ui.Read();

        IVehicle? vehicle = handler.Remove(registrationNumber);

        if (vehicle is not null)
        {
            ui.Write(vehicle.ToString());
        }
        else
        {
            ui.Write($"There is no vehicle with the registration number {registrationNumber} being parked at the moment.");
        }
    }

    private void SearchVehiclesByProperties()
    {
        PropertySelector propertySelector = new PropertySelector();
        SimpleMenu vehicleMenu = CreateVehicleMenu(propertySelector, false);
        vehicleMenu.AddMenuItem("Any vehicle", () =>
        {
            propertySelector[VehicleManager.PROP_TYPE] = "";
        });
        vehicleMenu.Run(ConsoleKey.Enter);

        string selectedType = propertySelector[VehicleManager.PROP_TYPE];
        if (selectedType == String.Empty)
        {
            selectedType = "vehicle";
        }

        //ui.Write(selectedType, ConsoleColor.Green);
        PropertyQuery(selectedType, VehicleManager.PROP_BRAND, propertySelector);
        PropertyQuery(selectedType, VehicleManager.PROP_COLOR, propertySelector);
        PropertyQuery(selectedType, VehicleManager.PROP_WHEELS, propertySelector);
        PropertyQuery(selectedType, VehicleManager.PROP_SPEC, propertySelector);
        //propertySelector.Display();

        int count = 0;
        
        foreach (IVehicle vehicle in handler)
        {
            string str;

            if (!DoesPropertySelectionMatch(propertySelector[VehicleManager.PROP_TYPE], vehicle.VehicleType)) continue;
            if (!DoesPropertySelectionMatch(propertySelector[VehicleManager.PROP_BRAND], vehicle.Brand)) continue;
            if (!DoesPropertySelectionMatch(propertySelector[VehicleManager.PROP_COLOR], vehicle.Color)) continue;
            if (!DoesPropertySelectionMatch(propertySelector[VehicleManager.PROP_WHEELS], vehicle.NumberOfWheels)) continue;

            object vehicleProperty;
            switch (vehicle.VehicleType)
            {
                case "Airplane":
                    vehicleProperty = ((Airplane)vehicle).FuelType;
                    break;
                case "Boat":
                    vehicleProperty = ((Boat)vehicle).Length;
                    break;
                case "Bus":
                    vehicleProperty = ((Bus)vehicle).NumberOfSeats;
                    break;
                case "Car":
                    vehicleProperty = ((Car)vehicle).EngineType;
                    break;
                case "Motorcycle":
                    vehicleProperty = ((Motorcycle)vehicle).SaddleType;
                    break;
                default:
                    throw new InvalidOperationException(nameof(vehicleProperty));
            }

            if (!DoesPropertySelectionMatch(propertySelector[VehicleManager.PROP_SPEC], vehicleProperty)) continue;

            ui.Write(vehicle.ToString());
            count++;
        }

        string amount = "";
        string plural = "";
        string type = "";
        string brand = "";
        string color = "";
        string wheels = "";

        amount = (count == 0) ? "no" : count.ToString();

        if (!String.IsNullOrEmpty(propertySelector[VehicleManager.PROP_TYPE]))
        {
            type = $"{propertySelector[VehicleManager.PROP_TYPE]}";
        }
        else
        {
            type = "vehicle";
        }
        if (count != 1)
        {
            if (type.EndsWith('s'))
            {
                plural = "es";
            }
            else
            {
                plural = "s";
            }
        }

        if (!String.IsNullOrEmpty(propertySelector[VehicleManager.PROP_BRAND]))
        {
            brand = $"of the brand '{propertySelector[VehicleManager.PROP_BRAND]}'";
        }
        else
        {
            brand = "of any brand";
        }
        if (!String.IsNullOrEmpty(propertySelector[VehicleManager.PROP_COLOR]))
        {
            color = $"with the color '{propertySelector[VehicleManager.PROP_COLOR]}'";
        }
        else
        {
            color = "of any color";
        }
        if (!String.IsNullOrEmpty(propertySelector[VehicleManager.PROP_WHEELS]))
        {
            wheels = $"with {propertySelector[VehicleManager.PROP_WHEELS]} number of wheels";
        }
        else
        {
            wheels = "of any number of wheels";
        }

        ui.Write($"There were {amount} {type}{plural} {brand} {color} {wheels}");
    }

    private void PropertyQuery(string vehicle, string property, PropertySelector propertySelector)
    {
        ui.Write($"What is the {vehicle}'s {property}? ");
        string propertyValue = ui.Read(false);
        propertySelector[property] = propertyValue;
    }

    private bool DoesPropertySelectionMatch(string selectedProperty, object vehicleProperty)
    {
        if (String.IsNullOrEmpty(selectedProperty))         // not selected equals match
            return true;

        if (vehicleProperty is string propStr)
        {
            if (!selectedProperty.Equals(propStr)) return false;
        }
        if (vehicleProperty is int propInt)
        {
            int selectInt;

            if (!int.TryParse(selectedProperty, out selectInt)) return false;
            if (selectInt != propInt) return false;
        }
        return true;
    }

    private bool DoesPropertySelectionMatch(string selectedProperty, int propertyNumber)
    {
        if (String.IsNullOrEmpty(selectedProperty))
            return true;

        int num;
        if (!int.TryParse(selectedProperty, out num))
            return false;

        if (num != propertyNumber)
            return false;

        return true;
    }

    //VehicleManager.PROP_TYPE
    private void PropertyHook(PropertySelector propertySelector)
    {
        if (propertySelector.Count() > 0)
        {
            ui.Write("Selected search criterion:", ConsoleColor.Cyan);
            foreach (KeyValuePair<string, string> kvp in propertySelector)
            {
                ui.Write($"{kvp.Key} {kvp.Value}", ConsoleColor.Green);
            }

            ui.Write("");
        }
    }

    private void SetGarageCapacity()
    {
        int numberOfSlots = 0;

        while (numberOfSlots < 1)
        {
            ui.Write("how many parking slots does your garage have? ");
            numberOfSlots = ui.ReadInt();
        }

        IEnumerable<IVehicle> parkedVehicles = handler;
        this.handler = new GarageHandler(numberOfSlots, parkedVehicles);
    }

    private void PopulateGarage()
    {
        if (this.handler.Count != 0)
        {
            Console.WriteLine(this.handler.ToString());
            throw new InvalidOperationException("Attempting to populate a populated garage.");
        }

        int count = (handler.Capacity - handler.Count) / 2;
        VehicleManager vehicleManager = new VehicleManager();

        for (int i = 0; i < count; i++)
        {
            IVehicle vehicle = vehicleManager.CreateRandomVehicle();
            this.handler.Add(vehicle);
        }
    }

    private void OldPopulateGarage()
    {
        if (this.handler.Count != 0)
        {
            Console.WriteLine(this.handler.ToString());
            throw new InvalidOperationException("Attemping to populate a populated garage.");
        }

        int count = (handler.Capacity - handler.Count) / 2;

        Type[] vehicleTypes =
            [
                typeof(Airplane),
                typeof(Boat),
                typeof(Bus),
                typeof(Car),
                typeof(Motorcycle)
            ];

        for (int i = 0; i < count; i++)
        {
            Type type = vehicleTypes[random.Next(vehicleTypes.Length)];
            string regNum = GetRegNum();
            Vehicle vehicle = (Vehicle)Activator.CreateInstance(type, regNum, "Yamaha", "Black", "1");
            this.handler.Add(vehicle: (IVehicle)vehicle);
        }
    }

    /// <summary>
    /// Generates a unique Registration Number
    /// </summary>
    /// <returns></returns>
    private string GetRegNum()
    {
        string regNum = $"A{registrationCounter++}";

        while (registrationNumbers.Contains(regNum))
        {
            regNum = $"A{registrationCounter++}";
        }

        return regNum;
    }
}
