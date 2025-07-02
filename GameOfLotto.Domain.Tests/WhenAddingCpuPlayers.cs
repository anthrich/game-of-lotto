namespace GameOfLotto.Domain.Tests;

public class WhenAddingCpuPlayers
{
    private readonly InMemoryPlayerRepository _playerRepository;
    private readonly PlayerManifest _playerManifest;
    private readonly Game _game = new(Guid.NewGuid());

    public WhenAddingCpuPlayers()
    {
        _playerRepository = new InMemoryPlayerRepository();
        _playerManifest = new PlayerManifest(_playerRepository, _game);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(15)]
    public void It_adds_the_correct_number_of_players(int numberOfPlayers)
    {
        // Act
        _playerManifest.AddCpuPlayers("Player", numberOfPlayers);
        
        // Assert
        var players = _playerRepository.Get();
        Assert.Equal(numberOfPlayers, players.Count);
    }

    [Fact]
    public void It_returns_the_new_players()
    {
        // Act
        var result = _playerManifest.AddCpuPlayers("Player", 15);
        
        // Assert
        var players = _playerRepository.Get();
        Assert.Equivalent(players, result);
    }

    [Fact]
    public void It_adds_the_correct_game_to_the_players()
    {
        // Act
        _playerManifest.AddCpuPlayers("Player", 15);
        
        // Assert
        var players = _playerRepository.Get();
        var playersHaveGameId = players.All(p => p.GameIds.Contains(_game.Id));
        Assert.True(playersHaveGameId);
    }
}