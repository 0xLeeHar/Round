using System.Text.Json;
using Ardalis.SmartEnum;

namespace Round.Common;

public class SmartEnumNameConverter<TEnum, TValue> : System.Text.Json.Serialization.JsonConverter<TEnum>
    where TEnum : SmartEnum<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>, IConvertible
{
    public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return (TEnum?)null;
                
            case JsonTokenType.String:
                return GetFromName(reader.GetString());

            default:
                throw new JsonException($"Unexpected token {reader.TokenType} when parsing a smart enum.");
        }
    }

    public override void Write(Utf8JsonWriter writer, TEnum? value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value.Name.ToString());
    }

    private TEnum? GetFromName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return (TEnum?)null;
        }
            
        try
        {
            return SmartEnum<TEnum, TValue>.FromName(name, false);
        }
        catch (Exception ex)
        {
            throw new JsonException($"Error converting value '{name}' to a smart enum.", ex);
        }
    }
}
