using Ardalis.SmartEnum;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Round.Common.Sql;

public static class PropertyConverters
{
    public static ValueConverter<TSmartEnum, TValue> CreateValueConverter<TSmartEnum, TValue>()
        where TSmartEnum : SmartEnum<TSmartEnum, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        return new(from => from.Value,
            to => SmartEnum<TSmartEnum, TValue>.FromValue(to));
    }
    
    public static ValueConverter<TSmartEnum, string> CreateNameConverter<TSmartEnum>()
        where TSmartEnum : SmartEnum<TSmartEnum>
    {
        return new(from => from.Name.ToLowerInvariant(),
            to => SmartEnum<TSmartEnum>.FromName(to, true));
    }
}