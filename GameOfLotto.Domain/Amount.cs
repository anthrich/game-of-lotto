namespace GameOfLotto.Domain;

public record Amount(string Currency, decimal Value)
{
    public Amount() : this("USD", 0) { }
};