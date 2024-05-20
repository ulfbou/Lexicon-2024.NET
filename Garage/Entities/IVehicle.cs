using Garage.Entities.Properties;

namespace Garage.Entities;

/// <summary>
/// IVehicle defines the common properties of all vehicles in a garage. 
/// </summary>
public interface IVehicle
{
    /// <summary>
    /// Gets the brand of the vehicle.
    /// </summary>
    BrandProperty Brand { get; }                   // A string representing a vehicle's brand.

    /// <summary>
    /// Gets the color of the vehicle.
    /// </summary>
    ColorProperty Color { get; }                   // A string representing a vehicle's color.

    /// <summary>
    /// Gets the number of wheels of a vehicle.
    /// </summary>
    NumberOfWheelsProperty NumberOfWheels { get; }             // An integer representing a vehicle's number of wheels.

    /// <summary>
    /// Gets the registration number of a vehicle.
    /// </summary>
    RegistrationNumberProperty RegistrationNumber { get; }      // A string representing a vehicle's registration number. 

    /// <summary>
    /// Gets the type of a vehicle.
    /// </summary>
    string VehicleType { get; }             // A string representing a vehicle's name. 
}
