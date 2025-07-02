using System.Globalization;

namespace GameOfLotto.Domain;

public record Amount(string Currency, decimal Value)
{
    private static readonly Dictionary<string, CultureInfo> IsoCodeToCultureInfo = new()
    {
        { "USD", CultureInfo.GetCultureInfo("en-US") },
    };
    
    public static Amount operator-(Amount a, Amount b)
    {
        return a with { Value = a.Value - b.Value };
    }

    public static bool operator >=(Amount a, Amount b)
    {
        return a.Value >= b.Value;
    }

    public static bool operator <=(Amount a, Amount b)
    {
        return a.Value <= b.Value;
    }

    public override string ToString()
    {
        return string.Format(IsoCodeToCultureInfo[Currency], "{0:C}", Value);
    }
};