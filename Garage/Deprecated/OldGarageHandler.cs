using Garage.Entities;
using System.Reflection;

namespace Garage.Deprecated;

/// <summary>
/// Class for handling the similation of the Garage.
/// </summary>
public class OldGarageHandler
{
    #region private fields
    private ConsoleUI UI = ConsoleUI.Instance;
    private Garage garage = null!;
    private Type[] vehicleTypes = [];
    private Dictionary<string, Type> vehicleTypeMap = new Dictionary<string, Type>();
    private Random random = new Random();
    private Dictionary<string, string[]> vehicleBrands = new Dictionary<string, string[]>();
    private string[] colors = ["Black", "White", "Red", "Green", "Blue", "Gray"];

    private readonly string PROP_TYPE = "Type";
    private readonly string PROP_BRAND = "Brand";
    private readonly string PROP_REGNUM = "RegNum";
    private readonly string PROP_COLOR = "Color";
    private readonly string PROP_WHEELS = "Wheels";
    private readonly string PROP_ARGS = "Args";

    private readonly string[] VEHICLE_ARG_TEXT = [];
    private readonly string[] AIRPLANE_ARG_VALUES = ["gasoline", "diesel", "biofuel", "etanol", "jetfuel"];
    private readonly string[] CAR_ARG_VALUES = ["petrol", "diesel", "electric", "hybrid", "hydrogen"];
    private readonly string[] MOTORCYCLE_ARG_VALUES = ["solo", "dual", "custom", "racing", "cruiser"];
    #endregion

    #region constructor

    /// <summary>
    /// Constructs a garage handler. 
    /// </summary>
    public OldGarageHandler()
    {
        vehicleTypes = GetInheritedTypes(typeof(Vehicle));
        vehicleBrands = new Dictionary<string, string[]>
        {
            {"Airplane", [ "Boeing, ", "Airbus", "Embraer, ", "Cessna", "Bombardier" ] },
            {"Boat", [ "Yamaha", "Bayliner", "Sea Ray", "Boston Whaler", "Chaparral" ] },
            {"Bus", [ "Volvo Buses", "Mercedes-Benz", "Scania", "Ashok Leyland", "Blue Bird" ] },
            {"Car", [ "Toyota", "Honda", "Ford", "Volkswagen", "BMW" ] },
            {"Motorcycles", [ "Harley-Davidson", "Yamaha", "Honda", "Kawasaki", "Ducati" ] }
        };

        SetGarageCapacity();
        Populate();

    }
    #endregion

    #region public methods
    public void ListAllParkedVehicles()
    {
        int count = 1;

        UI.Write("List of all parked vehicles:", ConsoleColor.White);

        foreach (IVehicle vehicle in garage)
        {
            UI.Write($"{vehicle}{Environment.NewLine}");
            count++;
        }

        if (count == 1)
        {
            UI.Write("The garage is empty!");
        }
    }

    public void ListVehicleTypes()
    {
        Dictionary<string, int> vehicleTypeCounter = new Dictionary<string, int>();

        foreach (IVehicle vehicle in garage)
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

        UI.Write("List of types of vehicles and their the amount of parked vehicles.", ConsoleColor.White);

        foreach (KeyValuePair<string, int> kvp in vehicleTypeCounter)
        {
            UI.Write($"{kvp.Key} {kvp.Value}");
        }
    }

    public void ParkCar()
    {
        if (garage.Count() == garage.Capacity())
        {
            UI.Write("The garage is full at the moment. Please, unpark another car before you try this again.", ConsoleColor.White);
            return;
        }

        Dictionary<string, string> propertySelector = new Dictionary<string, string>();
        SimpleMenu vehicleMenu = CreateVehicleMenu(propertySelector, null, false);
        vehicleMenu.Run(ConsoleKey.Enter);
        //UI.Write(propertySelector[PROP_TYPE], ConsoleColor.Green);

        UI.Write($"What is the {propertySelector[PROP_TYPE]}'s brand? ", ConsoleColor.White);
        string brand = UI.Read();
        propertySelector.Add(PROP_BRAND, brand);

        UI.Write($"What is the {propertySelector[PROP_TYPE]}'s registration number? ");
        string registrationName = UI.Read();
        propertySelector.Add(PROP_REGNUM, registrationName);

        UI.Write($"What is the {propertySelector[PROP_TYPE]}'s color? ");
        string color = UI.Read();
        propertySelector.Add(PROP_COLOR, color);

        // TODO: Ask for the name of the specific property instead of "specific property"!
        UI.Write($"What is the {propertySelector[PROP_TYPE]}'s specific property? ");
        string args = UI.Read();
        propertySelector.Add(PROP_ARGS, args);

        try
        {

            // TODO: Validate that this actually works.
            Type? type;
            if (vehicleTypeMap.ContainsKey(propertySelector[PROP_TYPE]))
            {
                type = vehicleTypeMap[propertySelector[PROP_TYPE]];
            }
            else
            {
                type = Type.GetType(propertySelector[PROP_TYPE]);
            }

            if (type is null)
            {
                UI.Write($"1. Could not create a {propertySelector[PROP_TYPE]}.");
                return;
            }

            object? obj = Activator.CreateInstance(type, propertySelector[PROP_REGNUM], propertySelector[PROP_BRAND], propertySelector[PROP_COLOR], propertySelector[PROP_ARGS]);

            if (obj is not null && obj is IVehicle vehicle)
            {
                garage.Add(vehicle);
                UI.Write($"A {vehicle.VehicleType} has now been parked.");
            }
            else
            {
                UI.Write($"2. Could not create a {propertySelector[PROP_TYPE]}.");
            }
        }
        catch (Exception ex)
        {
            UI.Write($"Error creating instance: {ex.Message}");
        }
    }

    public void RemoveVehicle(string? reg = "")
    { 
        UI.Write("Please, specify the registration number of the vehicle to be removed:");
        string registrationNumber = UI.Read();

        IVehicle? vehicle = garage.Remove(registrationNumber);

        if (vehicle is not null)
        {
            UI.Write($"{vehicle.VehicleType} with the registration number {registrationNumber} is no longer parking.", ConsoleColor.White);
        }
        else
        {
            UI.Write($"There is no vehicle parked by the registration number {registrationNumber}.", ConsoleColor.White);
        }
    }

    public void FindVehicleByRegNumber()
    {
        UI.Write("Which registration number are you looking for? ", ConsoleColor.White);

        string registrationNumber = UI.Read();

        IVehicle vehicle = garage.Remove(registrationNumber);

        if (vehicle is null)
        {
            UI.Write($"No vehicle with the registration number {registrationNumber} is being parked at the moment.");
        }
        else
        {
            UI.Write($"Removed the {vehicle.VehicleType} with the registration number {registrationNumber}");
        }
    }

    // TODO: Separate Property Handling into a separate class. 
    public void SearchVehiclesByProperties()
    {
        Dictionary<string, string> propertySelector = new Dictionary<string, string>();

        /* TODO Use a hook to display selected search criterion. */
        SimpleMenu menu = new SimpleMenu("Advanced vehicle search");

        SimpleMenu vehicleMenu = CreateVehicleMenu(propertySelector, menu);
        menu.AddSubMenu(vehicleMenu.Title, vehicleMenu);

        menu.AddMenuItem("Select the brand", () =>
        {
            UI.Write("What is the brand of the vehicle?");
            string brand = UI.Read();

            SetProperty(propertySelector, PROP_BRAND, brand);
            PropertyHook(propertySelector);
        }, false);
        menu.AddMenuItem("Select color", () =>
        {
            UI.Write("Which color should the vehicle have? ");
            string color = UI.Read();

            SetProperty(propertySelector, PROP_COLOR, color);
            PropertyHook(propertySelector);
        }, false);
        menu.AddMenuItem("Number of wheels", () =>
        {
            UI.Write("How many wheels should the vehicle have? ");
            int wheels = UI.ReadInt();

            SetProperty(propertySelector, PROP_WHEELS, wheels.ToString());
            PropertyHook(propertySelector);
        }, false);
        menu.AddMenuItem("Search by selected criterion", () =>
        {
            foreach (IVehicle vehicle in garage)
            {
                if (propertySelector.ContainsKey(PROP_TYPE))
                {
                    if (!propertySelector[PROP_TYPE].Equals(vehicle.VehicleType)) continue;
                }
                if (propertySelector.ContainsKey(PROP_BRAND))
                {
                    if (!propertySelector[PROP_BRAND].Equals(vehicle.Brand)) continue;
                }
                if (propertySelector.ContainsKey(PROP_COLOR))
                {
                    if (!propertySelector[PROP_COLOR].Equals(vehicle.Color)) continue;
                }
                if (propertySelector.ContainsKey(PROP_WHEELS))
                {
                    int wheels;
                    int.TryParse(propertySelector[PROP_WHEELS], out wheels);

                    if (wheels != vehicle.NumberOfWheels) continue;
                }

                UI.Write(vehicle.ToString());
            }

            PropertyHook(propertySelector);
        }, false);

        menu.Run();
    }

    public void SetGarageCapacity()
    {
        int capacity = 0;

        while (capacity < 1)
        {
            UI.Write("How many parking slots can be used at this garage? ", ConsoleColor.White);

            capacity = UI.ReadInt();

            if (capacity < 1)
            {
                UI.Write("You need to have at least one parking slot. Zero or less is not possible!");
            }
        }

        IVehicle[] resizedVehicles = new IVehicle[capacity];

        if (garage is not null)
        {
            IVehicle[] vehicles = garage.ToArray();
            Array.Copy(vehicles, resizedVehicles, Math.Min(vehicles.Length, capacity));
        }

        garage = new Garage(resizedVehicles);

        ListAllParkedVehicles();
        UI.Write("Please, press enter to continue.", ConsoleColor.White);
        UI.Read();
    }

    public IEnumerable<IVehicle> GetVehicles()
    {
        return garage;
    }

    #endregion

    #region private methods
    private SimpleMenu CreateVehicleMenu(Dictionary<string, string> propertySelector, SimpleMenu? parent, bool callHook = true)
    {
        SimpleMenu vehicleMenu = new SimpleMenu("Select type of vehicle", parent ?? throw new ArgumentException());

        foreach (Type vehicleType in vehicleTypes)
        {
            Vehicle vehicle = (Vehicle)Activator.CreateInstance(vehicleType);
            vehicleMenu.AddMenuItem(vehicle.VehicleType, () =>
            {
                SetProperty(propertySelector, PROP_TYPE, vehicle.VehicleType);
                // TODO: Add vehicle type specific search criterion. 

                if (callHook)
                {
                    PropertyHook(propertySelector);
                }
            }, false);
        }

        return vehicleMenu;
    }

    private Type[] GetInheritedTypes(Type baseType)
    {
        Assembly assembly = Assembly.GetExecutingAssembly(); // Använd rätt assembly om klasserna är i en annan fil
        Type[] allTypes = assembly.GetTypes().ToArray();

        Type[] inheritedTypes = allTypes
            .Where(type => baseType.IsAssignableFrom(type) && type != baseType)
            .ToArray();

        foreach (Type type in inheritedTypes)
        {
            var name = type.Name;
            vehicleTypeMap.Add(name, type);
        }

        return inheritedTypes;
    }

    private void SetProperty(Dictionary<string, string> propertySelectors, string propertyKey, string propertyValue)
    {
        if (!propertySelectors.ContainsKey(propertyKey))
        {
            propertySelectors.Add(propertyKey, propertyValue);
        }
        else
        {
            propertySelectors[propertyKey] = propertyValue;
        }
    }

    private void PropertyHook(Dictionary<string, string> propertySelectors)
    {
        if (propertySelectors.Count() > 0)
        {
            UI.Write("Selected search criterion:", ConsoleColor.Cyan);
            foreach (KeyValuePair<string, string> kvp in propertySelectors)
            {
                UI.Write($"{kvp.Value}", ConsoleColor.Green);
            }

            UI.Write("");
        }
    }

    private void Populate()
    {
        for (int i = 0; i < garage.Capacity(); i++)
        {
            garage.Add(CreateRandomVehicle(i));
        }

        ListAllParkedVehicles();
    }

    private IVehicle CreateRandomVehicle(int number)
    {
        int type = random.Next(vehicleTypes.Length);
        Type randomVehicleType = vehicleTypes[type];
        object?[] args = [
                GenerateRegistrationNumber(),
                GenerateTypeBrand(randomVehicleType.Name),
                GenerateColor(),
                GenerateTypeSpecificArg(randomVehicleType)
            ];
        IVehicle? vehicle;

        if (args[args.Length - 1] is int)
        {
            vehicle = (IVehicle?)Activator.CreateInstance(randomVehicleType, (string)args[0], (string)args[1], (string)args[2], (int)args[3]);
        }
        else
        {
            vehicle = (IVehicle?)Activator.CreateInstance(randomVehicleType, (string)args[0], (string)args[1], (string)args[2], (string)args[3]);
        }

        if (vehicle is null)
        {
            throw new InvalidOperationException("Can not generate {randomVehicleType.GetType().Name}");
        }

        return vehicle;
    }

    private object? GenerateTypeSpecificArg(Type type)
    {
        object? result;

        switch (type.Name)
        {
            case "Airplane":
                result = AIRPLANE_ARG_VALUES[random.Next(AIRPLANE_ARG_VALUES.Length)];
                break;
            case "Car":
                result = CAR_ARG_VALUES[random.Next(CAR_ARG_VALUES.Length)];
                break;
            case "Motorcycle":
                result = MOTORCYCLE_ARG_VALUES[random.Next(MOTORCYCLE_ARG_VALUES.Length)];
                break;
            default:            // Boat or Bus
                result = random.Next(10, 100);
                break;
        }

        return result;
    }

    private object GenerateColor()
    {
        return colors[random.Next(colors.Length)];
    }

    private string GenerateTypeBrand(string typeName)
    {

        if (vehicleBrands.ContainsKey(typeName))
        {
            string[] brands = vehicleBrands[typeName];
            return brands[random.Next(brands.Length)];
        }

        throw new ArgumentException(nameof(typeName));
    }

    private string GenerateRegistrationNumber()
    {
        Random random = new Random();
        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string digits = "0123456789";
        int len1 = letters.Length;
        int len2 = digits.Length;

        return $"{letters[random.Next(len1)]}{letters[random.Next(len1)]}{letters[random.Next(len1)]}{digits[random.Next(len2)]}{digits[random.Next(len2)]}{digits[random.Next(len2)]}";
    }
    #endregion
}
