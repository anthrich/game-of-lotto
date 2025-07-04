﻿
namespace GameOfLotto.Domain.Tests;

public class WhenPurchasingTickets
{
    private readonly Player _player;
    private readonly InMemoryPlayerRepository _playerRepository;
    private readonly TicketOffice _ticketOffice;

    public WhenPurchasingTickets()
    {
        _player = new Player("Player 1", new Amount("USD", 10), Guid.NewGuid());
        _playerRepository = new InMemoryPlayerRepository();
        _playerRepository.Save(_player);
        _ticketOffice = new TicketOffice(_playerRepository);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void The_player_will_receive_the_correct_amount_of_tickets(uint numberOfTickets)
    {
        // Act
        _ticketOffice.Purchase(_player.Id, numberOfTickets);
        
        // Assert
        var persistedPlayer = _playerRepository.GetById(_player.Id);
        Assert.Equal((uint)persistedPlayer.Tickets.Count, numberOfTickets);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void The_player_will_have_their_balance_reduced(uint numberOfTickets)
    {
        // Act
        _ticketOffice.Purchase(_player.Id, numberOfTickets);
        
        // Assert
        var persistedPlayer = _playerRepository.GetById(_player.Id);
        Assert.Equal(new Amount("USD", 10 - numberOfTickets), persistedPlayer.Balance);
    }
    
    [Theory]
    [InlineData(100)]
    [InlineData(50)]
    [InlineData(11)]
    public void The_player_can_not_buy_more_than_their_balance_allows(uint numberOfTickets)
    {
        // Act
        _ticketOffice.Purchase(_player.Id, numberOfTickets);
        
        // Assert
        var persistedPlayer = _playerRepository.GetById(_player.Id);
        Assert.Equal(10, persistedPlayer.Tickets.Count);
    }
}