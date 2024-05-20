using Garage.Entities.Properties;

namespace Garage.Entities;

/// <summary>
/// Represents a car, inheriting from the <see cref="Vehicle"/> class.
/// </summary>
public class Car : Vehicle
{
    #region properties
    /// <summary>
    /// Gets or sets the number of seats of the car. 
    /// </summary>
    public EngineTypeProperty EngineType { get; private set; }
    #endregion

    #region constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Car"/> class with specified parameters.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the car.</param>
    /// <param name="brand">The brand of the car.</param>
    /// <param name="color">The color of the car.</param>
    /// <param name="engineType">The engine type of the car.</param>
    public Car(
        RegistrationNumberProperty registrationNumber,
        BrandProperty brand,
        ColorProperty color,
        EngineTypeProperty engineType)
        : base(registrationNumber, brand, color, 6, "Car")
    {
        this.EngineType = engineType;
    }

#if IMPLICITCONSTRUCTOR
    /// <summary>
    /// Initializes a new instance of the <see cref="Car"/> class with specified parameters.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the car.</param>
    /// <param name="brand">The brand of the car.</param>
    /// <param name="color">The color of the car.</param>
    /// <param name="engineType">The engine type of the car.</param>
    public Car(
        string registrationNumber, 
        string brand, 
        string color, 
        string engineType)
        : base(registrationNumber, brand, color, 6, "Car")
    {
        this.EngineType = engineType;
    }
#endif
#endregion

    #region method
    /// <summary>
    /// Returns a string that represents the current car object.
    /// </summary>
    /// <returns>A string that represents the current car object.</returns>
    public override string ToString()
    {
        return base.ToString() + $"{Environment.NewLine}Engine Type: {this.EngineType}";
    }
    #endregion
}
