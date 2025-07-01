namespace GameOfLotto.Domain.Tests;

public class WhenAddingAPlayer
{
    private readonly InMemoryPlayerRepository _playerRepository;
    private readonly PlayerManifest _playerManifest;

    public WhenAddingAPlayer()
    {
        _playerRepository = new InMemoryPlayerRepository();
        _playerManifest = new PlayerManifest(_playerRepository);
    }

    [Fact]
    public void It_persists_the_player()
    {
        // Act
        _playerManifest.AddPlayer("Player 1", new Game());
        
        // Assert
        var expected = new Player("Player 1");
        var actual = _playerRepository.Saved[0];
        Assert.Equivalent(expected, actual);
    }

    [Fact]
    public void It_adds_the_game_to_the_players_games()
    {
        // Arrange
        var game = new Game(Guid.NewGuid());
        
        // Act
        _playerManifest.AddPlayer("Player 1", game);
        
        // Assert
        var player = _playerRepository.Saved[0];
        Assert.Contains(player.GameIds, gId => gId == game.Id);
    }
}