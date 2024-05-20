using Garage.Entities.Properties;

namespace Garage.Entities;

/// <summary>
/// Represents a motorcycle, inheriting from the <see cref="Vehicle"/> class.
/// </summary>
public class Motorcycle : Vehicle
{
    #region properties
    /// <summary>
    /// Gets or sets the number of seats of the motorcycle. 
    /// </summary>
    public SaddleTypeProperty SaddleType { get; private set; }
    #endregion

    #region constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Motorcycle"/> class with specified parameters.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the motorcycle.</param>
    /// <param name="brand">The brand of the motorcycle.</param>
    /// <param name="color">The color of the motorcycle.</param>
    /// <param name="saddleType">The engine type of the motorcycle.</param>
    public Motorcycle(
        RegistrationNumberProperty registrationNumber,
        BrandProperty brand,
        ColorProperty color,
        SaddleTypeProperty saddleType)
        : base(registrationNumber, brand, color, 6, "Motorcycle")
    {
        this.SaddleType = saddleType;
    }

#if IMPLICITCONSTRUCTOR
    /// <summary>
    /// Initializes a new instance of the <see cref="Motorcycle"/> class with specified parameters.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the motorcycle.</param>
    /// <param name="brand">The brand of the motorcycle.</param>
    /// <param name="color">The color of the motorcycle.</param>
    /// <param name="saddleType">The engine type of the motorcycle.</param>
    public Motorcycle(
        string registrationNumber,
        string brand,
        string color,
        string saddleType)
        : base(registrationNumber, brand, color, 6, "Motorcycle")
    {
        this.SaddleType = saddleType;
    }
#endif
    #endregion

    #region method
    /// <summary>
    /// Returns a string that represents the current motorcycle object.
    /// </summary>
    /// <returns>A string that represents the current motorcycle object.</returns>
    public override string ToString()
    {
        return base.ToString() + $"{Environment.NewLine}Saddle Type: {this.SaddleType}";
    }
    #endregion
}
