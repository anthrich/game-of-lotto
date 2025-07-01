namespace GameOfLotto.Domain.Tests;

public class WhenAddingAPlayer
{
    [Fact]
    public void The_manifest_persists_the_player()
    {
        // Arrange
        var playerRepository = new InMemoryPlayerRepository();
        var manifest = new PlayerManifest(playerRepository);
        
        // Act
        manifest.AddPlayer("Player 1");
        
        // Assert
        var expected = new Player("Player 1");
        var actual = playerRepository.Saved[0];
        Assert.Equivalent(expected, actual);
    }
}