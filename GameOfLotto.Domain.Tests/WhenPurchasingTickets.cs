
namespace GameOfLotto.Domain.Tests;

public class WhenPurchasingTickets
{
    private readonly Player _player;
    private readonly InMemoryPlayerRepository _playerRepository;
    private readonly TicketOffice _ticketOffice;

    public WhenPurchasingTickets()
    {
        _player = new Player("Player 1", Guid.NewGuid());
        _playerRepository = new InMemoryPlayerRepository();
        _playerRepository.Save(_player);
        _ticketOffice = new TicketOffice(_playerRepository);
    }

    [Fact]
    public void The_player_will_receive_a_single_ticket()
    {
        // Act
        _ticketOffice.Purchase(_player.Id, 1);
        
        // Assert
        var persistedPlayer = _playerRepository.Saved[0];
        Assert.Single(persistedPlayer.Tickets);
    }
}