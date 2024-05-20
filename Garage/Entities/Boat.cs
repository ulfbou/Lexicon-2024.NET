using Garage.Entities.Properties;

namespace Garage.Entities;

/// <summary>
/// Represents a boat, inheriting from the <see cref="Vehicle"/> class.
/// </summary>
public class Boat : Vehicle
{
    #region properties
    /// <summary>
    /// Gets or sets the length of the boat. 
    /// </summary>
    public LengthProperty Length { get; private set; }
    #endregion

    #region constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Boat"/> class with specified parameters.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the boat.</param>
    /// <param name="brand">The brand of the boat.</param>
    /// <param name="color">The color of the boat.</param>
    /// <param name="length">The length of the boat.</param>
    public Boat(
        RegistrationNumberProperty registrationNumber, 
        BrandProperty brand, 
        ColorProperty color, 
        LengthProperty length)
        : base(registrationNumber, brand, color, 6, "Boat")
    {
        this.Length = length;
    }

    #if IMPLICITCONSTRUCTOR
    /// <summary>
    /// Initializes a new instance of the <see cref="Boat"/> class with specified parameters.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the boat.</param>
    /// <param name="brand">The brand of the boat.</param>
    /// <param name="color">The color of the boat.</param>
    /// <param name="length">The length of the boat.</param>
    public Boat(
        string registrationNumber, 
        string brand, 
        string color, 
        int length)
        : base(registrationNumber, brand, color, 6, "Boat")
    {
        this.Length = length;
    }
#endif
#endregion

    #region method
    /// <summary>
    /// Returns a string that represents the current boat object.
    /// </summary>
    /// <returns>A string that represents the current boat object.</returns>
    public override string ToString()
    {
        return base.ToString() + $"{Environment.NewLine}Length: {this.Length}";
    }
    #endregion
}
