namespace GameOfLotto.Domain;

public interface IPlayerRepository
{
    public void Save(Player player);
    public Player GetById(Guid id);
}