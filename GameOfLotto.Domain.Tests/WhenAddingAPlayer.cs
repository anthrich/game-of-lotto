namespace GameOfLotto.Domain.Tests;

public class WhenAddingAPlayer
{
    private readonly InMemoryPlayerRepository _playerRepository;
    private readonly PlayerManifest _playerManifest;
    private readonly Game _game = new(Guid.NewGuid());

    public WhenAddingAPlayer()
    {
        _playerRepository = new InMemoryPlayerRepository();
        _playerManifest = new PlayerManifest(_playerRepository, _game);
    }

    [Fact]
    public void It_sets_the_players_name()
    {
        // Act
        var newPlayer = _playerManifest.AddPlayer("Player 1");
        
        // Assert
        Assert.Equal("Player 1", newPlayer.Name);
    }
    
    [Fact]
    public void It_adds_the_game_to_the_players_games()
    {
        // Act
        var player = _playerManifest.AddPlayer("Player 1");
        
        // Assert
        Assert.Contains(player.GameIds, gId => gId == _game.Id);
    }

    [Fact]
    public void It_persists_the_player()
    {
        // Act
        _playerManifest.AddPlayer("Player 1");
        
        // Assert
        var expected = new { Name = "Player 1", GameIds = new List<Guid> { _game.Id } };
        var actual = _playerRepository.Saved[0];
        Assert.Equivalent(expected, actual);
    }
}