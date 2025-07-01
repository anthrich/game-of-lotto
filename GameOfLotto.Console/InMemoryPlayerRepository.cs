using Force.DeepCloner;
using GameOfLotto.Domain;

namespace GameOfLotto;

public class InMemoryPlayerRepository : IPlayerRepository
{
    public List<Player> Saved { get; private set; } = [];

    public void Save(Player player)
    {
        var copiedPlayer = player.DeepClone();
        Saved = Saved.Where(p => p.Id != player.Id).ToList();
        Saved.Add(copiedPlayer);
    }

    public Player GetById(Guid id)
    {
        return Saved.First(p => p.Id == id).DeepClone();
    }
}