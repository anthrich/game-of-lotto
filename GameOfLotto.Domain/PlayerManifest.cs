namespace GameOfLotto.Domain;

public class PlayerManifest(IPlayerRepository repository, Game game)
{
    public Player AddPlayer(string name)
    {
        var player = new Player(name, new Amount("USD", 10));
        player.AddGame(game.Id);
        repository.Save(player);
        return player;
    }

    public IReadOnlyCollection<Player> AddCpuPlayers(string name, int numberOfCpuPlayers)
    {
        var players = new List<Player>();
        
        for (var i = 1; i <= numberOfCpuPlayers; i++)
        {
            players.Add(AddPlayer(name + " " + i));
        }

        return players;
    }
}