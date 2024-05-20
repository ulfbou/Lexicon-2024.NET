using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Entities.Properties;

interface IProperty
{
    string Name { get; }
    Type ValueType { get; }

}

public class Property<T>(string Name, T Value) : IProperty
{
    public string Name { get; } = Name;
    public T Value { get; } = Value;
    public Type ValueType { get; } = typeof(T);

}

public class StringProperty(string Name, string Value) : Property<string>(Name, Value) 
{
    #region implicit operator
    public static implicit operator StringProperty(string Value)
    {
        return new StringProperty("string", Value);
    }

    public static implicit operator string(StringProperty Value)
    {
        return Value.Value;
    }
    #endregion
}

public class IntegerProperty(string Name, int Value) : Property<int>(Name, Value)
{
    #region implicit operator
    public static implicit operator IntegerProperty(int Value)
    {
        return new IntegerProperty("integer", Value);
    }

    public static implicit operator int(IntegerProperty Value)
    {
        return Value.Value;
    }
    #endregion
}

public class RegistrationNumberProperty(string Name, string Value) : StringProperty(Name, Value)
{
    #region implicit operator
    public static implicit operator RegistrationNumberProperty(string Value)
    {
        return new RegistrationNumberProperty("string", Value);
    }

    public static implicit operator string(RegistrationNumberProperty Value)
    {
        return Value.Value;
    }
    #endregion
}

public class BrandProperty(string Name, string Value) : StringProperty(Name, Value)
{
    #region implicit operator
    public static implicit operator BrandProperty(string Value)
    {
        return new BrandProperty("string", Value);
    }

    public static implicit operator string(BrandProperty Value)
    {
        return Value.Value;
    }
    #endregion
}

public class ColorProperty(string Name, string Value) : StringProperty(Name, Value)
{
    #region implicit operator
    public static implicit operator ColorProperty(string Value)
    {
        return new ColorProperty("string", Value);
    }

    public static implicit operator string(ColorProperty Value)
    {
        return Value.Value;
    }
    #endregion
}

public class FuelTypeProperty(string Name, string Value) : StringProperty(Name, Value)
{
    #region implicit operator
    public static implicit operator FuelTypeProperty(string Value)
    {
        return new FuelTypeProperty("string", Value);
    }

    public static implicit operator string(FuelTypeProperty Value)
    {
        return Value.Value;
    }
    #endregion
}

public class EngineTypeProperty(string Name, string Value) : StringProperty(Name, Value)
{
    #region implicit operator
    public static implicit operator EngineTypeProperty(string Value)
    {
        return new EngineTypeProperty("string", Value);
    }

    public static implicit operator string(EngineTypeProperty Value)
    {
        return Value.Value;
    }
    #endregion
}

public class SaddleTypeProperty(string Name, string Value) : StringProperty(Name, Value)
{
    #region implicit operator
    public static implicit operator SaddleTypeProperty(string Value)
    {
        return new SaddleTypeProperty("saddle type", Value);
    }

    public static implicit operator string(SaddleTypeProperty Value)
    {
        return Value.Value;
    }
    #endregion
}


public class NumberOfWheelsProperty(
    string Name,
    int Value)
    : IntegerProperty(Name, Value)
{
    #region implicit operator
    public static implicit operator NumberOfWheelsProperty(int Value)
    {
        return new NumberOfWheelsProperty("numberOfWheels", Value);
    }

    public static implicit operator int(NumberOfWheelsProperty Value)
    {
        return Value.Value;
    }
    #endregion
}

public class LengthProperty(
    string Name,
    int Value)
    : IntegerProperty(Name, Value)
{
    #region implicit operator
    public static implicit operator LengthProperty(int Value)
    {
        return new LengthProperty("length", Value);
    }

    public static implicit operator int(LengthProperty Value)
    {
        return Value.Value;
    }
    #endregion
}

public class NumberOfSeatsProperty(
    string Name,
    int Value)
    : IntegerProperty(Name, Value)
{
    #region implicit operator
    public static implicit operator NumberOfSeatsProperty(int Value)
    {
        return new NumberOfSeatsProperty("number of seats", Value);
    }

    public static implicit operator int(NumberOfSeatsProperty Value)
    {
        return Value.Value;
    }
    #endregion
}

