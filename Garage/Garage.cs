using System.Collections;
using System.Linq;
using Garage.Entities;

namespace Garage;

/// <summary>
/// Represents a generic garage capable of parking vehicles of type T, implementing IEnumerable of T.
/// </summary>
/// <typeparam name="T">The type of vehicles the garage can store, must implement IVehicle.</typeparam>
public class Garage<T> : IEnumerable<T> where T : IVehicle
{
    #region fields
    private T?[] vehicles;
    private HashSet<string> registrationNumbers = null!;
    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Garage{T}"/> class with a specified capacity.
    /// </summary>
    /// <param name="capacity">The capacity of the garage.</param>
    public Garage(int capacity)
    {
        vehicles = new T?[capacity];
        registrationNumbers = new HashSet<string>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Garage{T}"/> class with specified vehicles.
    /// </summary>
    /// <param name="vehicles">The vehicles to be parked in the garage.</param>
    public Garage(IEnumerable<T> vehicles)
    {
        this.vehicles = vehicles.ToArray();
        registrationNumbers = new HashSet<string>();
    }
    #endregion

    #region methods

    /// <summary>
    /// Gets the number of vehicles currently parked in the garage.
    /// </summary>
    public int Count
    {
        get
        {
            if (this.vehicles is null) return 0;
            int count = 0;
            var enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is null) continue;
                count++;
            }
            return count;
        }
    }


    /// <summary>
    /// Gets the capacity of the garage.
    /// </summary>
    public int Capacity => vehicles.Length;

    /// <summary>
    /// Checks if a vehicle is parked in the garage.
    /// </summary>
    /// <param name="vehicle">The vehicle to be checked.</param>
    /// <returns>True if the vehicle is parked, otherwise false.</returns>
    public bool IsVehicleParked(T vehicle)
    {
        return IsVehicleParked(vehicle.RegistrationNumber);
    }

    /// <summary>
    /// Checks if a vehicle with the specified registration number is parked in the garage.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the vehicle to be checked.</param>
    /// <returns>True if the vehicle with the specified registration number is parked, otherwise false.</returns>
    public bool IsVehicleParked(string registrationNumber)
    {
        int slot = FindParkingSlotByRegistrationNumber(registrationNumber);
        return slot >= 0;
    }

    /// <summary>
    /// Parks a vehicle in the garage.
    /// </summary>
    /// <param name="vehicle">The vehicle to park.</param>
    /// <returns>True, if the vehicle is successfully parked. Otherwise, false.</returns>
    public bool Add(T vehicle)
    {
        if (vehicle is null)
        {
            throw new ArgumentNullException(nameof(vehicle));
        }

        int slotIndex = FindFirstEmptySlot();

        if (slotIndex == vehicles.Length)
        {
            return false;
        }

        string registrationNumber = vehicle.RegistrationNumber.Value.ToLower();

        vehicles[slotIndex] = vehicle;
        registrationNumbers.Add(registrationNumber);

        return true;
    }

    /// <summary>
    /// Removes a vehicle from the garage based on its registration number.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the vehicle to remove.</param>
    /// <returns>The removed vehicle, or null if the vehicle was not found.</returns>
    public IVehicle? Remove(string registrationNumber)
    {
        int slot = FindParkingSlotByRegistrationNumber(registrationNumber);

        IVehicle? vehicle = (slot < 0) ? null : vehicles[slot];

        if (registrationNumbers.Remove(registrationNumber.ToLower()))
        {
            vehicles[slot] = default(T);
        }

        return vehicle;
    }

    private int FindFirstEmptySlot()
    {
        int slot = 0;

        while (slot < vehicles.Length && vehicles[slot] is not null)
        {
            slot++;
        }

        return slot;
    }

    private int FindParkingSlotByRegistrationNumber(string registrationNumber)
    {
        string reg = registrationNumber.ToLower();

        if (!registrationNumbers.Contains(reg))
        {
            return -1;
        }

        for (int slot = 0; slot < vehicles.Length; slot++)
        {
            if (!(vehicles[slot] is null) && vehicles[slot].RegistrationNumber.Value.ToLower().Equals(reg))
            {
                return slot;
            }
        }

        return -1;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the vehicles in the garage.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the vehicles in the garage.</returns>
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return vehicles.Where(p => p is not null);
        //for (int i = 0; i < vehicles.Length; i++)
        //{
        //    if (vehicles[i] is null) continue;
        //    yield return vehicles[i]!;
        //}
    }

    /// <summary>
    /// Returns an enumerator that iterates through the vehicles in the garage.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the vehicles in the garage.</returns>
    public IEnumerator GetEnumerator()
    {
        return vehicles.GetEnumerator();
    }

    #endregion
}
