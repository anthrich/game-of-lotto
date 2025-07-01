namespace GameOfLotto.Domain;

public interface IPlayerRepository
{
    public void Save(Player player);
}