namespace GameOfLotto.Domain.Tests;

public class WhenRunningTheGame
{
    private readonly Game _game;
    private readonly InMemoryPlayerRepository _playerRepo;
    private readonly PlayerManifest _playerManifest;
    private readonly TicketOffice _ticketOffice;
    private readonly Player[] _players;
    private const int Seed = 1234;
    // given the seed, the first random of 1-20 tickets will be 8
    private const int SeededWinningTicketNumber = 8;

    public WhenRunningTheGame()
    {
        _game = new Game();
        _playerRepo = new InMemoryPlayerRepository();
        _playerManifest = new PlayerManifest(_playerRepo, _game);
        _ticketOffice = new TicketOffice(_playerRepo);
        _players = _playerManifest.AddCpuPlayers("Player", 10).ToArray();
        foreach (var player in _players)
        {
            _ticketOffice.Purchase(player.Id, 2);
        }
    }

    [Fact]
    public void The_first_ticket_drawn_will_be_awarded_the_grand_prize()
    {
        // Arrange
        var winner = _players[SeededWinningTicketNumber / 2 - 1]; 

        // Act
        var result = _game.Run(_playerRepo, Seed);
        
        // Assert
        Assert.Equivalent(new[] { winner.Id }, result.GrandPrize.Winners.Select(p => p.Id));
    }

    [Fact]
    public void The_grand_prize_will_be_50_percent_of_the_ticket_revenue()
    {
        // Act
        var result = _game.Run(_playerRepo, Seed);
        
        // Assert
        Assert.Equal(new Amount("USD", 10), result.GrandPrize.Value);
    }
}