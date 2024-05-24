using Newtonsoft.Json;

namespace TournamentAPI;

public class DateFormatConverter : Newtonsoft.Json.JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is DateTime)
        {
            var dateTime = (DateTime)value;
            writer.WriteValue(dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss")); // customize this format as needed
        }
        else
        {
            throw new NotImplementedException("Only DateTime is supported");
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
    }

    public override bool CanRead => false;

    public override bool CanConvert(Type objectType) => objectType == typeof(DateTime);
}
