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

    public IReadOnlyCollection<Player> AddCpuPlayers(string name, int numberOfCpuPlayers)
    {
        var players = new List<Player>();
        
        for (var i = 0; i < numberOfCpuPlayers; i++)
        {
            players.Add(AddPlayer(name + " " + i));
        }

        return players;
    }
}