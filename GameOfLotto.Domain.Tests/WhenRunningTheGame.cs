namespace GameOfLotto.Domain.Tests;

public class WhenRunningTheGame
{
    [Fact]
    public void The_first_ticket_drawn_will_be_awarded_the_grand_prize()
    {
        // Arrange
        var game = new Game();
        var playerRepo = new InMemoryPlayerRepository();
        var playerManifest = new PlayerManifest(playerRepo, game);
        var ticketOffice = new TicketOffice(playerRepo);
        var players = playerManifest.AddCpuPlayers("Player", 10).ToArray();
        
        foreach (var player in players)
        {
            ticketOffice.Purchase(player.Id, 2);
        }

        var seed = 1234;
        var seededWinningTicketNumber = 8;
        var winner = players[seededWinningTicketNumber / 2 - 1]; 

        // Act
        var result = game.Run(playerRepo, seed);
        
        // Assert
        Assert.Equivalent(new[] { winner.Id }, result.GrandPrize.Winners.Select(p => p.Id));
    }
}