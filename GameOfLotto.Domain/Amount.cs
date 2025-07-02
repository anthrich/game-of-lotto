namespace GameOfLotto.Domain;

public record Amount(string Currency, decimal Value)
{
    public static Amount operator -(Amount a, Amount b)
    {
        return a with { Value = a.Value - b.Value };
    }
};