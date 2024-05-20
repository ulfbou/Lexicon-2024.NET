
using Garage.Entities;
using System.Collections;

namespace Garage;

/// <summary>
/// Represents a handler for managing vehicles in a garage, where T is a type of vehicle.
/// </summary>
/// <typeparam name="T">The type of vehicles managed by the garage handler, must implement IVehicle.</typeparam>
public class GarageHandler<T> : IEnumerable<T> where T : IVehicle
{
    private readonly Garage<T> garage;


    /// <summary>
    /// Initializes a new instance of the <see cref="GarageHandler{T}"/> class with a specified number of slots.
    /// </summary>
    /// <param name="numberOfSlots">The number of slots in the garage.</param>
    public GarageHandler(int numberOfSlots)
    {
        this.garage = new Garage<T>(numberOfSlots);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GarageHandler{T}"/> class with a specified number of slots and vehicles.
    /// </summary>
    /// <param name="numberOfSlots">The number of slots in the garage.</param>
    /// <param name="vehicles">The vehicles to be added to the garage.</param>
    public GarageHandler(int numberOfSlots, IEnumerable<T> vehicles)
    {
        T[] vehicleArray = vehicles.ToArray();
        int capacity = Math.Min(vehicleArray.Length, numberOfSlots);
        this.garage = new Garage<T>(numberOfSlots);

        for (int i = 0; i < capacity; i++)
        {
            this.garage.Add(vehicleArray[i]);
        }
    }

    /// <summary>
    /// Gets the number of vehicles currently parked in the garage.
    /// </summary>
    public int Count => garage.Count;

    /// <summary>
    /// Gets the capacity of the garage.
    /// </summary>
    public int Capacity => garage.Capacity;

    /// <summary>
    /// Adds a vehicle to the garage.
    /// </summary>
    /// <param name="vehicle">The vehicle to add.</param>
    /// <returns>True if the vehicle is successfully added, otherwise false.</returns>
    public bool Add(T vehicle)
    {
        return this.garage.Add(vehicle);
    }

    /// <summary>
    /// Removes a vehicle from the garage based on its registration number.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the vehicle to remove.</param>
    /// <returns>The removed vehicle, or null if the vehicle was not found.</returns>
    public T? Remove(string registrationNumber)
    {
        return (T?)this.garage.Remove(registrationNumber);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the vehicles in the garage.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the vehicles in the garage.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        foreach (T vehicle in garage)
        {
            if (vehicle is null)
            {
                continue;
            }

            yield return vehicle;
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the vehicles in the garage.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the vehicles in the garage.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


/// <summary>
/// Represents a handler for managing vehicles in a garage, where the vehicles are of type IVehicle.
/// </summary>
public class GarageHandler : GarageHandler<IVehicle>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GarageHandler"/> class with a specified number of slots.
    /// </summary>
    /// <param name="numberOfSlots">The number of slots in the garage.</param>
    public GarageHandler(int numberOfSlots) : base(numberOfSlots) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GarageHandler"/> class with a specified number of slots and vehicles.
    /// </summary>
    /// <param name="numberOfSlots">The number of slots in the garage.</param>
    /// <param name="vehicles">The vehicles to be added to the garage.</param>
    public GarageHandler(int numberOfSlots, IEnumerable<IVehicle> vehicles) : base(numberOfSlots, vehicles) { }
}






//public class GarageHandler : IEnumerable<IVehicle>
//{
//    private readonly Garage<IVehicle> garage;

//    public GarageHandler(int numberOfSlots)
//    {
//        this.garage = new Garage<IVehicle>(numberOfSlots);
//    }

//    public GarageHandler(int numberOfSlots, IEnumerable<IVehicle> vehicles)
//    {
//        IVehicle[] vehicleArray = vehicles.ToArray();
//        int capacity = Math.Min(vehicleArray.Length, numberOfSlots);
//        this.garage = new Garage<IVehicle>(numberOfSlots);

//        for(int i = 0; i < capacity; i++)
//        {
//            this.garage.Add(vehicleArray[i]);
//        }
//    }

//    public int Count => garage.Count;
//    public int Capacity => garage.Capacity;

//    public bool Add(IVehicle vehicle)
//    {
//        return this.garage.Add(vehicle);
//    }

//    public IVehicle? Remove(string registrationNumber)
//    {
//        return this.garage.Remove(registrationNumber);
//    }

//    public IEnumerator<IVehicle> GetEnumerator()
//    {
//        foreach(IVehicle vehicle in garage)
//        {
//            if (vehicle is null)
//            {
//                continue;
//            }

//            yield return vehicle;
//        }
//    }

//    IEnumerator IEnumerable.GetEnumerator()
//    {
//        return GetEnumerator();
//    }
//}