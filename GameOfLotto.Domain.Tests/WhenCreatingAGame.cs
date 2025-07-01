namespace GameOfLotto.Domain.Tests;

public class WhenCreatingAGame
{
    [Fact]
    public void There_are_no_players()
    {
        // Act
        var game = new Game();
        
        // Assert
        var players = game.GetPlayers();
        Assert.Empty(players);
    }
}