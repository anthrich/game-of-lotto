namespace GameOfLotto.Domain;

public class Player(string name)
{
    public string Name = name;
    public readonly List<Guid> GameIds = new();

    internal void AddGame(Guid gameId)
    {
        GameIds.Add(gameId);
    }
};