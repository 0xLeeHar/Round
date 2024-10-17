using System.Text.Json;
using System.Text.Json.Serialization;
using Round.Common.Domain;

namespace Round.Common;

public class BankAccountAddressConverter : JsonConverter<BankAccountAddress>
{
    public override BankAccountAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;
            
            case JsonTokenType.String:
                return new BankAccountAddress { Value = reader.GetString() ?? "" };

            default:
                throw new JsonException($"Unexpected token {reader.TokenType} when parsing a smart enum.");
        }
    }

    public override void Write(Utf8JsonWriter writer, BankAccountAddress? value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value.Value.ToString());
    }
}
