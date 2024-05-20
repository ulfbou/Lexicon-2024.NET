using Garage.Entities.Properties;
using System;

namespace Garage.Entities;

/// <summary>
/// Represents a general vehicle with common properties and functionality.  
/// </summary>
public abstract class Vehicle {
    //#region fields
    //private readonly RegistrationNumberProperty registrationNumberProperty = registrationNumber;
    //private readonly BrandProperty brandProperty = brand;
    //private readonly ColorProperty colorProperty = color;
    //private readonly string vehicleType = vehicleType;
    //private readonly NumberOfWheelsProperty numberOfWheelsProperty = numberOfWheels;
    //#endregion

    #region properties
    /// <summary>
    /// Gets or sets the registration number of a vehicle.
    /// </summary>
    public RegistrationNumberProperty RegistrationNumber { get; protected set; }

    /// <summary>
    /// Gets or sets the brand of a vehicle.
    /// </summary>
    public BrandProperty Brand { get; protected set; }

    /// <summary>
    /// Gets or sets the color of a vehicle.
    /// </summary>
    public ColorProperty Color { get; protected set; }

    /// <summary>
    /// Gets or sets the number of wheels of a vehicle. 
    /// </summary>
    public NumberOfWheelsProperty NumberOfWheels { get; protected set; }

    /// <summary>
    /// Gets or sets the type of the vehicle.
    /// </summary>
    public string VehicleType { get; protected set; }
    #endregion

    #region constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Vehicle"/> class with a specified vehicle type.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the vehicle.</param>
    /// <param name="brand">The brand of the vehicle.</param>
    /// <param name="color">The color of the vehicle.</param>
    /// <param name="numberOfWheels">The number of wheels the vehicle has.</param>
    /// <param name="vehicleType">The type of the vehicle.</param>
    public Vehicle(
        RegistrationNumberProperty registrationNumber, 
        BrandProperty brand, 
        ColorProperty color, 
        NumberOfWheelsProperty numberOfWheels, 
        string vehicleType)
    {
        RegistrationNumber = registrationNumber ?? throw new ArgumentNullException(nameof(registrationNumber));
        Brand = brand ?? throw new ArgumentNullException(nameof(brand));
        Color = color ?? throw new ArgumentNullException(nameof(color));
        NumberOfWheels = numberOfWheels ?? throw new ArgumentNullException(nameof(numberOfWheels));
        VehicleType = vehicleType;
    }
    #endregion

    #region methods
    /// <summary>
    /// Returns a string that represents the current vehicle object.
    /// </summary>
    /// <returns>A string that represents the current vehicle object.</returns>
    public override string ToString()
    {
        return $"Vehicle Type: {VehicleType}{Environment.NewLine}Registration Number: {RegistrationNumber}{Environment.NewLine}Brand: {Brand}{Environment.NewLine}Color: {Color}{Environment.NewLine}Number of wheels: {NumberOfWheels}";
    }
    #endregion

}
