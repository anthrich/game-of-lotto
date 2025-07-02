namespace GameOfLotto.Domain;

public class PlayerManifest(IPlayerRepository repository, Game game)
{
    public Player AddPlayer(string name)
    {
        var player = new Player(name);
        player.AddGame(game.Id);
        repository.Save(player);
        return player;
    }

    public void AddCpuPlayers(string name, int numberOfCpuPlayers)
    {
        for (var i = 0; i < numberOfCpuPlayers; i++)
        {
            AddPlayer(name + " " + i);
        }
    }
}