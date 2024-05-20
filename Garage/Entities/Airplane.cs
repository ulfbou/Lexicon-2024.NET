using Garage.Entities.Properties;

namespace Garage.Entities;

/// <summary>
/// Represents an airplane, inheriting from the <see cref="Vehicle"/> class.
/// </summary>
public class Airplane : Vehicle
{
    #region properties
    /// <summary>
    /// Gets or sets the fuel type of the airplane. 
    /// </summary>
    public FuelTypeProperty FuelType { get; private set; }
    #endregion

    #region constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Airplane"/> class with specified parameters.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the airplane.</param>
    /// <param name="brand">The brand of the airplane.</param>
    /// <param name="color">The color of the airplane.</param>
    /// <param name="fuelType">The fuel type of the airplane.</param>
    public Airplane(
        RegistrationNumberProperty registrationNumber,
        BrandProperty brand,
        ColorProperty color,
        FuelTypeProperty fuelType)
        : base(registrationNumber, brand, color, 6, "Airplane")
    {
        this.FuelType = fuelType ?? throw new ArgumentNullException(nameof(fuelType));
    }

#if IMPLICITCONSTRUCTOR
    /// <summary>
    /// Initializes a new instance of the <see cref="Airplane"/> class with specified parameters.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the airplane.</param>
    /// <param name="brand">The brand of the airplane.</param>
    /// <param name="color">The color of the airplane.</param>
    /// <param name="fuelType">The fuel type of the airplane.</param>
    public Airplane(string registrationNumber, string brand, string color, string fuelType)
        : base(registrationNumber, brand, color, 6, "Airplane")
    {
        this.FuelType = fuelType ?? throw new ArgumentNullException(nameof(fuelType));
    }
#endif
#endregion

    #region method
    /// <summary>
    /// Returns a string that represents the current airplane object.
    /// </summary>
    /// <returns>A string that represents the current airplane object.</returns>
    public override string ToString()
    {
        return base.ToString() + $"{Environment.NewLine}Fuel type: {this.FuelType}";
    }
    #endregion
}
