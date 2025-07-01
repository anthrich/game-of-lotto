namespace GameOfLotto.Domain;

public class PlayerManifest(IPlayerRepository repository)
{
    public void AddPlayer(string name, Game game)
    {
        var player = new Player(name);
        player.AddGame(game.Id);
        repository.Save(player);
    }
}