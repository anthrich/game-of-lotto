namespace GameOfLotto.Domain;

public class PlayerManifest(IPlayerRepository repository)
{
    public void AddPlayer(string name)
    {
        repository.Save(new Player(name));
    }
}