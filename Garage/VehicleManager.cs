using Garage;
using Garage.Entities;
using Garage.Entities.Properties;
using System.Reflection;

// TODO: Merge/consolidate constructors and other data

///// <summary>
///// Manages vehicle types and provides methods to discover and retrieve them.
///// </summary>
public class VehicleManager
{
    // Constants representing common properties of vehicles.
    public const string PROP_TYPE = "type"!;
    public const string PROP_BRAND = "brand"!;
    public const string PROP_COLOR = "color"!;
    public const string PROP_WHEELS = "wheels"!;
    public const string PROP_SPEC = "spec"!;
    public const string PROP_REGNUM = "regnum";

    private Dictionary<string, Type>? vehicleTypes;
    private Dictionary<string, ConstructorInfo>? vehicleConstructors;
    private Dictionary<string, ParameterInfo[]>? vehicleConstructorParameters;

    private Logger logger = Logger.Instance;

    private Dictionary<string, Type> VehicleTypes
    {
        get
        {
            if (vehicleTypes is null)
            {
                // Initialize vehicle types and constructors
                InitializeVehicleReflection();
            }

            return vehicleTypes!;
        }
    }

    private Dictionary<string, ConstructorInfo> VehicleConstructor
    {
        get
        {
            if (vehicleConstructors is null)
            {
                // Initialize vehicle types and constructors
                InitializeVehicleReflection();
            }

            return vehicleConstructors!;
        }
    }

    private Dictionary<string, ParameterInfo[]> VehicleConstructorParameters
    {
        get
        {
            if (vehicleConstructorParameters is null)
            {
                // Initialize vehicle types and constructors
                InitializeVehicleReflection();
            }

            return vehicleConstructorParameters!;
        }
    }

    private void InitializeVehicleReflection()
    {
        vehicleTypes = new Dictionary<string, Type>();
        vehicleConstructors = new Dictionary<string, ConstructorInfo>();
        vehicleConstructorParameters = new Dictionary<string, ParameterInfo[]>();

        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] allTypes = assembly.GetTypes();

        foreach (Type vehicleType in allTypes)
        {
            if (vehicleType.IsAssignableTo(typeof(Vehicle)) && !vehicleType.IsAbstract && !vehicleType.IsInterface)
            {
                string vehicleTypeName = vehicleType.Name;

                vehicleTypes.Add(vehicleTypeName, vehicleType);

                ConstructorInfo[] allConstructors = vehicleType.GetConstructors();

                if (allConstructors.Length != 1)
                {
                    // Log error: More than one constructor found for the vehicle type
                    logger.Log(LogLevel.Error, $"Error: More than one constructor found for {vehicleTypeName}. Fallback mechanism used.");
                    continue; // Skip this type and move to the next one
                }

                ConstructorInfo constructor = allConstructors[0];
                ParameterInfo[] parameters = constructor.GetParameters();

                if (parameters.Length == 0)
                {
                    // Log error: No parameters found for the constructor
                    logger.Log(LogLevel.Error, $"Error: No parameters found for the constructor of {vehicleTypeName}. Fallback mechanism used.");
                    continue; // Skip this type and move to the next one
                }

                vehicleConstructors.Add(vehicleTypeName, constructor);
                vehicleConstructorParameters.Add(vehicleTypeName, parameters);
                logger.Log(LogLevel.Info, $"Adding {vehicleType.Name}");
            }
            else
            {
                //logger.Log(LogLevel.Info, $"Ignoring {vehicleType.Name}");
            }
        }
    }

    // Implement other methods here...

    public IVehicle CreateRandomVehicle()
    {
        Random random = new Random();

        // Get a random vehicle type
        string randomVehicleType = VehicleTypes.Keys.ElementAt(random.Next(VehicleTypes.Count));

        // Create a random property selector for the vehicle type
        PropertySelector propertySelector = CreateRandomPropertySelector(randomVehicleType);

        // Create the vehicle using the selected type and property selector
        IVehicle vehicle = CreateVehicle(randomVehicleType, propertySelector);

        if (vehicle is null)
        {
            throw new InvalidOperationException(nameof(vehicle));
        }

        return vehicle;
    }

    public IVehicle CreateVehicle(string selectedType, PropertySelector propertySelector)
    {
        Logger logger = Logger.Instance;

        // Check if the selected type exists in the vehicle types dictionary
        if (!VehicleTypes.ContainsKey(selectedType))
        {
            logger.Log(LogLevel.Error, $"Error: Vehicle type '{selectedType}' not found.");
            return null;
        }

        Type vehicleType = VehicleTypes[selectedType];

        // Check if the selected type has a corresponding constructor
        if (!VehicleConstructor.ContainsKey(selectedType))
        {
            logger.Log(LogLevel.Error, $"Error: No constructor found for vehicle type '{selectedType}'.");
            return null;
        }

        ConstructorInfo constructor = VehicleConstructor[selectedType];
        ParameterInfo[] parameters = constructor.GetParameters();

        // Validate the property selector against the constructor parameters
        if (!ValidatePropertySelector(propertySelector, parameters))
        {
            logger.Log(LogLevel.Error, $"Error: Invalid property selector for vehicle type '{selectedType}'.");
            return null;
        }

        // Extract property values from the property selector
        object[] parameterValues = ExtractPropertyValues(propertySelector, parameters);

        try
        {
            // Create an instance of the vehicle using reflection
            object instance = constructor.Invoke(parameterValues);

            if (instance is IVehicle vehicle)
            {
                return vehicle;
            }
            else
            {
                logger.Log(LogLevel.Error, $"Error: Unable to create vehicle instance for type '{selectedType}'.");
                return null;
            }
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, $"Error: Exception occurred while creating vehicle instance for type '{selectedType}': {ex.Message}");
            return null;
        }
    }

    // Helper method to create a random property selector for a given vehicle type
    private PropertySelector CreateRandomPropertySelector(string vehicleType)
    {
        Random random = new Random();
        PropertySelector propertySelector = new PropertySelector();

        // Get the parameters for the constructor of the given vehicle type
        ParameterInfo[] parameters = VehicleConstructorParameters[vehicleType];

        // Populate the property selector with random values for each parameter
        foreach (ParameterInfo parameter in parameters)
        {
            object randomValue = GenerateRandomValue(parameter.ParameterType);
            propertySelector[parameter.Name] = randomValue?.ToString();
        }

        return propertySelector;
    }

    // Helper method to generate random values for property selector
    private object GenerateRandomValue(Type type)
    {
        Random random = new Random();
        Type stringProperty = typeof(StringProperty);
        Logger log = Logger.Instance;
        log.SetVisibleToConsoleLevel(LogLevel.Info);
 
        if (stringProperty.IsAssignableFrom(type))
        {
            // Generate a random string
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string str = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            object obj = (string)Activator.CreateInstance(type, str);
            return obj;
        }
        else if (typeof(IntegerProperty).IsAssignableFrom(type))
        {
            // Generate a random integer
            int number = random.Next(1, 100);
            object obj = Activator.CreateInstance(type, number);
            return (int)obj;
        }
        else if (type == typeof(bool))
        {
            // Generate a random boolean
            return random.Next(2) == 0;
        }
        else
        {
            // Unsupported type
            log.Log(LogLevel.Error, $"Unsupported type: {type}");
            return null;
        }
    }

    // Helper method to validate property selector against constructor parameters
    private bool ValidatePropertySelector(PropertySelector propertySelector, ParameterInfo[] parameters)
    {
        // Check if all parameter names in the selector exist in the constructor parameters
        foreach (ParameterInfo parameter in parameters)
        {
            if (propertySelector[parameter.Name!] is null)
            {
                return false;
            }
        }
        return true;
    }

    // Helper method to extract property values from property selector
    private object[] ExtractPropertyValues(PropertySelector propertySelector, ParameterInfo[] parameters)
    {
        List<object> parameterValues = new List<object>();

        foreach (ParameterInfo parameter in parameters)
        {
            string value = propertySelector[parameter.Name];
            parameterValues.Add(value);
        }

        return parameterValues.ToArray();
    }

    internal IEnumerable<ParameterInfo> GetConstructorParameters(string selectedType)
    {
        if (vehicleConstructorParameters.ContainsKey(selectedType))
        {
            return vehicleConstructorParameters[selectedType];
        }
        else
        {
            Logger.Instance.Log(LogLevel.Error, $"Error: Vehicle type '{selectedType}' not found.");
            return Enumerable.Empty<ParameterInfo>();
        }
    }

    internal Type? GetVehicleType(string selectedType)
    {
        if (vehicleTypes.ContainsKey(selectedType))
        {
            return vehicleTypes[selectedType];
        }
        else
        {
            Logger.Instance.Log(LogLevel.Error, $"Error: Vehicle type '{selectedType}' not found.");
            return null;
        }
    }

    internal IEnumerable<string> GetVehicleTypes()
    {
        return vehicleTypes.Keys;
    }
}

public class OldVehicleManager
{
#if OLDSTUFF
    // Dictionary to map vehicle type names to their corresponding types.
    private readonly Dictionary<string, Type> vehicleTypeMap = new Dictionary<string, Type>();

    // Dictionary to map vehicle type names to their corresponding properties.
    private readonly Dictionary<string, Dictionary<string, Type>> vehicleProperties = new Dictionary<string, Dictionary<string, Type>>();

    //  Dictionary to map vehicle type names to their corresponding constructors. 
    private readonly Dictionary<string, ConstructorInfo> vehicleConstructors = new Dictionary<string, ConstructorInfo>();

    // Constants representing common properties of vehicles.
    public const string PROP_TYPE = "type"!;
    public const string PROP_BRAND = "brand"!;
    public const string PROP_COLOR = "color"!;
    public const string PROP_WHEELS = "wheels"!;
    public const string PROP_SPEC = "spec"!;
    public const string PROP_REGNUM = "regnum";

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleManager"/> class.
    /// </summary>
    public VehicleManager()
    {
        DiscoverVehicleTypesAndProperties();
        InitializeVehicleConstructors();
        if (vehicleTypeMap.Count == 0)
        {
            throw new InvalidOperationException(nameof(vehicleTypeMap));
        }
        if (vehicleProperties.Count == 0)
        {
            throw new InvalidOperationException(nameof(vehicleProperties));
        }
        if (vehicleConstructors.Count == 0)
        {
            throw new InvalidOperationException(nameof(vehicleConstructors));
        }
    }

    /// <summary>
    /// Discovers vehicle types and their properties defined in the executing assembly and adds them to the type map.
    /// </summary>
    private void DiscoverVehicleTypesAndProperties()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] allTypes = assembly.GetTypes();

        foreach (Type type in allTypes)
        {
            if (typeof(IVehicle).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            {
                // Add vehicle type
                vehicleTypeMap.Add(type.Name, type);

                // Discover properties derived from Property<T>
                PropertyInfo[] properties = type.GetProperties();
                Dictionary<string, Type> propertyMap = properties
                    .Where(p => typeof(IProperty).IsAssignableFrom(p.PropertyType))
                    .ToDictionary(p => p.Name, p => p.PropertyType);

                vehicleProperties.Add(type.Name, propertyMap);
            }
        }
    }
    private void OldInitializeVehicleConstructors()
    {
        // Assuming GetVehicleTypes() returns a collection of vehicle types
        foreach (string vehicleType in GetVehicleTypes())
        {
            Type? type = GetVehicleType(vehicleType);
            if (type != null)
            {
                ConstructorInfo constructor = type.GetConstructors().FirstOrDefault();
                if (constructor != null)
                {
                    vehicleConstructors.Add(vehicleType, constructor);
                }
            }
        }
    }

    private void InitializeVehicleConstructors()
    {
        // Assuming GetVehicleTypes() returns a collection of vehicle types
        foreach (string vehicleType in GetVehicleTypes())
        {
            Type? type = vehicleTypeMap.TryGetValue(vehicleType, out Type foundType) ? foundType : null;
            if (type != null)
            {
                ConstructorInfo[] constructors = type.GetConstructors();
                if (constructors.Length > 0)
                {
                    // Select the first constructor for simplicity, you might need to handle multiple constructors differently
                    ConstructorInfo constructor = constructors[0];
                    vehicleConstructors.Add(vehicleType, constructor);
                }
            }
        }
    }

    public IVehicle? CreateVehicle(string vehicleType, PropertySelector propertySelector)
    {
        if (propertySelector is null)
        {
            // Skicka standardvärden som parametrar istället för att kasta ett undantag
            propertySelector = new PropertySelector();
        }

        if (vehicleConstructors.TryGetValue(vehicleType, out ConstructorInfo constructor))
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            List<object?> parameterValues = new List<object?>();

            foreach (ParameterInfo parameter in parameters)
            {
                if (propertySelector[parameter.Name!] is string value && value != null)
                {
                    if (parameter.ParameterType.IsAssignableFrom(typeof(string)))
                    {
                        parameterValues.Add(value);
                    }
                    else if (parameter.ParameterType == typeof(int) && int.TryParse(value, out int intValue))
                    {
                        parameterValues.Add(intValue);
                    }
                    else
                    {
                        parameterValues.Add(parameter.DefaultValue);
                    }
                }
                else
                {
                    parameterValues.Add(parameter.DefaultValue);
                }
            }

            try
            {
                object? obj = constructor.Invoke(parameterValues.ToArray());

                if (obj is IVehicle vehicle)
                {
                    return vehicle;
                }
            }
            catch (Exception ex)
            {
                // Logga eller hantera eventuella undantag under instansskapelsen
            }
        }

        return null;
    }

    public IVehicle? CreateRandomVehicle()
    {
        // Get a list of vehicle types from the existing vehicleConstructors dictionary
        List<string> vehicleTypes = vehicleConstructors.Keys.ToList();

        // Randomly select a vehicle type from the list
        Random random = new Random();
        int vcount = vehicleTypes.Count;
        int rnum = random.Next(vcount);
        
        string randomVehicleType = vehicleTypes[rnum];

        // Get the constructor for the selected vehicle type
        ConstructorInfo constructor = vehicleConstructors[randomVehicleType];
        ParameterInfo[] parameters = constructor.GetParameters();

        // Create a PropertySelector object to hold properties for the random vehicle
        PropertySelector propertySelector = new PropertySelector();

        // Add random properties for each parameter
        foreach (ParameterInfo parameter in parameters)
        {
            // Generate a random value for each parameter
            object randomValue = GenerateRandomValue(parameter.ParameterType);
            propertySelector[parameter.Name] = randomValue?.ToString();
        }

        // Create the vehicle using the vehicleType and propertySelector
        IVehicle vehicle = CreateVehicle(randomVehicleType, propertySelector);

        return vehicle;
    }

    // Metod för att generera slumpmässiga värden baserat på parametertyp
    private object GenerateRandomValue(Type parameterType)
    {
        Random random = new Random();
        object randomValue = null!;

        if (parameterType == typeof(string))
        {
            // Generera en slumpmässig sträng
            randomValue = Guid.NewGuid().ToString();
        }
        else if (parameterType == typeof(int))
        {
            // Generera ett slumpmässigt heltal
            randomValue = random.Next();
        }
        else if (parameterType == typeof(double))
        {
            // Generera ett slumpmässigt decimaltal
            randomValue = random.NextDouble();
        }
        // Fortsätt med fler datatyper beroende på behov

        return randomValue;
    }


    public IVehicle? NewCreateVehicle(string vehicleType, PropertySelector propertySelector)
    {
        if (propertySelector is null)
        {
            throw new ArgumentNullException(nameof(propertySelector));
        }

        if (vehicleConstructors.TryGetValue(vehicleType, out ConstructorInfo constructor))
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            List<object?> parameterValues = new List<object?>();

            foreach (ParameterInfo parameter in parameters)
            {
                if (parameter is null)
                {
                    throw new InvalidOperationException(nameof(parameter));
                }

                // Check if the parameter exists in the propertySelector
                if (propertySelector[parameter.Name!] is not null)
                {
                    object? value = propertySelector[parameter.Name!];

                    // Convert the value to the parameter's type
                    if (value != null && parameter.ParameterType.IsAssignableFrom(value.GetType()))
                    {
                        parameterValues.Add(value);
                    }
                    else
                    {
                        // Log or handle conversion errors
                        // For example, you can use default values or skip this parameter
                        // based on your requirements
                        // Handle conversion errors
                    }
                }
                else if (parameter.ParameterType == typeof(NumberOfWheelsProperty))
                {
                    // Handle NumberOfWheelsProperty separately
                    parameterValues.Add(new NumberOfWheelsProperty((int)parameter.DefaultValue));
                }
                else
                {
                    // Handle missing parameters
                    // For example, use default values or skip this parameter
                    // based on your requirements
                }
            }

            // Create an instance of the vehicle using the constructor and parameter values
            try
            {
                object? obj = constructor.Invoke(parameterValues.ToArray());

                if (obj is IVehicle vehicle)
                {
                    return vehicle;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions during instance creation
            }
        }

        return null;
    }

    public IVehicle OldCreateVehicle(string vehicleType, PropertySelector propertySelector)
    {
        if (vehicleConstructors.TryGetValue(vehicleType, out ConstructorInfo constructor))
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            List<object?> parameterValues = new List<object?>();

            foreach (ParameterInfo parameter in parameters)
            {
                if (parameter.Name != "numberOfWheels")
                {
                    // Assuming the property selector contains the necessary values
                    parameterValues.Add(Convert.ChangeType(propertySelector[parameter.Name], parameter.ParameterType));
                }
            }

            // Create an instance of the vehicle using the constructor and parameter values
            object? obj = constructor.Invoke(parameterValues.ToArray());

            if (obj is IVehicle vehicle)
            {
                return vehicle;
            }
        }

        return null;
    }

    /// <summary>
    /// Retrieves the names of all discovered vehicle types.
    /// </summary>
    /// <returns>An enumerable collection of vehicle type names.</returns>
    public IEnumerable<string> GetVehicleTypes()
    {
        return vehicleTypeMap.Keys;
    }

    /// <summary>
    /// Retrieves the type of the vehicle with the specified type name.
    /// </summary>
    /// <param name="vehicleType">The name of the vehicle type.</param>
    /// <returns>The type of the vehicle, or null if the type is not found.</returns>
    public Type GetVehicleType(string vehicleType)
    {
        return vehicleTypeMap.ContainsKey(vehicleType) ? vehicleTypeMap[vehicleType] : null;
    }

    /// <summary>
    /// Retrieves the properties of the vehicle with the specified type name.
    /// </summary>
    /// <param name="vehicleType">The name of the vehicle type.</param>
    /// <returns>The properties of the vehicle, or null if the type is not found.</returns>
    public Dictionary<string, Type> GetVehicleProperties(string vehicleType)
    {
        return vehicleProperties.ContainsKey(vehicleType) ? vehicleProperties[vehicleType] : null;
    }

    // TODO: Add other methods 
#endif
}
