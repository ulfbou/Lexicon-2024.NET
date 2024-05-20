using System.Collections;

namespace Garage;

/// <summary>
/// Represents a property selector, allowing key-value pairs to be stored and accessed.
/// </summary>
public class PropertySelector : IEnumerable<KeyValuePair<string, string>>
{
    // Dictionary to store key-value pairs.
    private readonly Dictionary<string, string> propertySelector = new Dictionary<string, string>()!;

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <returns>The value associated with the specified key, or null if the key is not found.</returns>
    public string this[string key]
    {
        get => propertySelector.ContainsKey(key) ? propertySelector[key] : null;
        set => propertySelector[key] = value;
    }

    /// <summary>
    /// Clears all key-value pairs from the property selector.
    /// </summary>
    public void Clear()
    {
        propertySelector.Clear();
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return propertySelector.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Displays all key-value pairs in the property selector.
    /// </summary>
    public void Display()
    {
        foreach (var item in propertySelector)
        {
            if (String.IsNullOrEmpty(item.Value)) continue;
            Console.WriteLine($"{item.Key}, {item.Value}");
        }
    }
}



//internal class PropertySelector : IEnumerable<KeyValuePair<string, string>>
//{
//    private readonly Dictionary<string, string> properties = new Dictionary<string, string>();

//    // Indexer för att möjliggöra användning av this[index] för att få eller sätta egenskaper
//    public string this[string key]
//    {
//        get
//        {
//            if (properties.ContainsKey(key))
//            {
//                return properties[key];
//            }
//            else
//            {
//                return string.Empty;
//            }
//        }
//        set
//        {
//            if (!properties.ContainsKey(key))
//            {
//                properties.Add(key, value);
//            }
//            else
//            {
//                properties[key] = value;
//            }
//        }
//    }

//    // Metod för att skapa fordon baserat på valda egenskaper
//    public Object?[] GetProperties()
//    {
//        object? typeInstance;

//        try
//        {
//            if (vehicleTypeMap.ContainsKey(type))
//            {
//                Type vehicleType = vehicleTypeMap[type];
//                typeInstance = Activator.CreateInstance(vehicleType, regNum, brand, color, specArg);

//                if (obj is IVehicle vehicle)
//                {
//                    garage.Add(vehicle);
//                    ui.Write($"A {vehicle.VehicleType} has now been parked.");
//                    return vehicle;
//                }
//            }

//            ui.Write($"Could not create a vehicle of type {type}.");
//        }
//        catch (Exception ex)
//        {
//            ui.Write($"Error creating vehicle: {ex.Message}", ConsoleColor.Red);
//        }

//        return
//            [
//                //this[GarageManager.PROP_TYPE],
//                this[GarageManager.PROP_REGNUM],
//                this[GarageManager.PROP_BRAND],
//                this[GarageManager.PROP_COLOR],
//                this[GarageManager.PROP_SPEC]
//            ];
//    }
//    public IVehicle CreateVehicle(Dictionary<string, Type> vehicleTypeMap, List<IVehicle> garage, SimpleUI ui)
//    {
//        string type    = this[GarageManager.PROP_TYPE  ];
//        string brand   = this[GarageManager.PROP_BRAND ];
//        string color   = this[GarageManager.PROP_COLOR ];
//        string regNum  = this[GarageManager.PROP_REGNUM];
//        string specArg = this[GarageManager.PROP_SPEC  ];

//        try
//        {
//            if (vehicleTypeMap.ContainsKey(type))
//            {
//                Type vehicleType = vehicleTypeMap[type];
//                object obj = Activator.CreateInstance(vehicleType, regNum, brand, color, specArg);

//                if (obj is IVehicle vehicle)
//                {
//                    garage.Add(vehicle);
//                    ui.Write($"A {vehicle.VehicleType} has now been parked.");
//                    return vehicle;
//                }
//            }

//            ui.Write($"Could not create a vehicle of type {type}.");
//        }
//        catch (Exception ex)
//        {
//            ui.Write($"Error creating vehicle: {ex.Message}", ConsoleColor.Red);
//        }

//        return null;
//    }

//    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
//    {
//        return properties.GetEnumerator();
//    }

//    IEnumerator IEnumerable.GetEnumerator()
//    {
//        throw new NotImplementedException();
//    }
//}



