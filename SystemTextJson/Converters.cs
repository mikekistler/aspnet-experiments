using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
            DateTimeOffset.Parse(reader.GetString()!);

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeOffset dateTimeValue,
        JsonSerializerOptions options) =>
            writer.WriteStringValue(dateTimeValue.UtcDateTime.ToString(
                "yyyy-MM-ddTHH:mm:ss.fffK", CultureInfo.InvariantCulture));
}

public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
            DateTime.Parse(reader.GetString()!);

    public override void Write(
        Utf8JsonWriter writer,
        DateTime dateTimeValue,
        JsonSerializerOptions options) =>
            writer.WriteStringValue(dateTimeValue.ToUniversalTime().ToString(
                "yyyy-MM-ddTHH:mm:ss.fffK", CultureInfo.InvariantCulture));
}
