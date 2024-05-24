using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace TournamentAPI;

public class CustomDateContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);

        if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
        {
            property.Converter = new DateFormatConverter(); // use the converter we created before
        }

        return property;
    }
}
