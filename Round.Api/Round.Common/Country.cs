using System.Text.Json.Serialization;
using Ardalis.SmartEnum;

namespace Round.Common;

[JsonConverter(typeof(SmartEnumNameConverter<Country, int>))]
public class Country : SmartEnum<Country>
{
    public static readonly Country XX = new(0, "Unknown", "XX");
    
    public static readonly Country GB = new(826, "United Kingdom", "GB");
    public static readonly Country FR = new(250, "France", "FR");
    public static readonly Country DE = new(276, "Germany", "DE");
    
    //TODO: Add other country specific props here, like BIC/IBAN format, default currency, etc.
    public Country(int value, string name, string iso2) : base(iso2, value)
    {
        CountryName = name;
    }
    
    public string CountryName { get; }
}
