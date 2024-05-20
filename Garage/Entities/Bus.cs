using Garage.Entities.Properties;

namespace Garage.Entities;

/// <summary>
/// Represents a bus, inheriting from the <see cref="Vehicle"/> class.
/// </summary>
public class Bus : Vehicle
{
    #region properties
    /// <summary>
    /// Gets or sets the number of seats of the bus. 
    /// </summary>
    public NumberOfSeatsProperty NumberOfSeats { get; private set; }
    #endregion

    #region constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="Bus"/> class with specified parameters.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the bus.</param>
    /// <param name="brand">The brand of the bus.</param>
    /// <param name="color">The color of the bus.</param>
    /// <param name="numberOfSeats">The number of seats of the bus.</param>
    public Bus(
        RegistrationNumberProperty registrationNumber,
        BrandProperty brand,
        ColorProperty color,
        NumberOfSeatsProperty numberOfSeats)
        : base(registrationNumber, brand, color, 6, "Bus")
    {
        this.NumberOfSeats = numberOfSeats;
    }

#if IMPLICITCONSTRUCTOR
    /// <summary>
    /// Initializes a new instance of the <see cref="Bus"/> class with specified parameters.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the bus.</param>
    /// <param name="brand">The brand of the bus.</param>
    /// <param name="color">The color of the bus.</param>
    /// <param name="numberOfSeats">The number of seats of the bus.</param>
    public Bus(
        string registrationNumber, 
        string brand, 
        string color, 
        int numberOfSeats)
        : base(registrationNumber, brand, color, 6, "Bus")
    {
        this.NumberOfSeats = numberOfSeats;
    }
#endif
    #endregion

    #region method
    /// <summary>
    /// Returns a string that represents the current bus object.
    /// </summary>
    /// <returns>A string that represents the current bus object.</returns>
    public override string ToString()
    {
        return base.ToString() + $"{Environment.NewLine}Number of Seats: {this.NumberOfSeats}";
    }
    #endregion
}
