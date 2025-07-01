namespace GameOfLotto.Domain.Tests;

public class InMemoryPlayerRepository : IPlayerRepository
{
    public List<Player> Saved { get; } = [];

    public void Save(Player player)
    {
        Saved.Add(player);
    }
}