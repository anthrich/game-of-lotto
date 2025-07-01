
namespace GameOfLotto.Domain.Tests;

public class WhenPurchasingTickets
{
    [Fact]
    public void The_player_will_receive_a_single_ticket()
    {
        // Arrange
        var player = new Player("Player 1", Guid.NewGuid());
        var playerRepository = new InMemoryPlayerRepository();
        playerRepository.Save(player);
        var ticketOffice = new TicketOffice(playerRepository);

        // Act
        ticketOffice.Purchase(player.Id, 1);
        
        // Assert
        var persistedPlayer = playerRepository.Saved[0];
        Assert.Single(persistedPlayer.Tickets);
    }
}