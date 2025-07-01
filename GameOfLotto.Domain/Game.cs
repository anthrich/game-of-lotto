namespace GameOfLotto.Domain;

public class Game(Guid id = default)
{
    public Guid Id { get; private set; } = id;

    public IList<object> GetPlayers()
    {
        return [];
    }
}
