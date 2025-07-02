namespace GameOfLotto.Domain.Tests;

public class WhenRunningTheGame
{
    private readonly Game _game;
    private readonly InMemoryPlayerRepository _playerRepo;
    private readonly PlayerManifest _playerManifest;
    private readonly TicketOffice _ticketOffice;
    private readonly Player[] _players;
    private readonly Ticket[] _tickets;
    private const int Seed = 1234;
    // given the seed, the first random of 1-20 tickets will be 8
    private readonly int _seededWinningTicketNumber;
    private readonly int[] _seededSecondTierWinningTicketNumbers = [0, 0];

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

        _players = _playerRepo.Get().ToArray();
        _tickets = _players.SelectMany(p => p.Tickets).ToArray();
        var random = new Random(Seed);
        _seededWinningTicketNumber = random.Next(1, 20);
        _seededSecondTierWinningTicketNumbers[0] = random.Next(1, 19);
        _seededSecondTierWinningTicketNumbers[1] = random.Next(1, 18);
    }

    [Fact]
    public void The_first_ticket_drawn_will_award_the_grand_prize_to_its_owner()
    {
        // Act
        var result = _game.Run(_playerRepo, Seed);
        
        // Assert
        var winner = _players[_seededWinningTicketNumber / 2 - 1]; 
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

    [Fact]
    public void The_next_2_tickets_will_be_awarded_the_second_tier_prizes()
    {
        
        // Act
        var result = _game.Run(_playerRepo, Seed);
        
        // Assert
        var winningTicketOne = _tickets[_seededSecondTierWinningTicketNumbers[0] - 1];
        var winningTicketTwo = _tickets[_seededSecondTierWinningTicketNumbers[1] - 1];
        var winnerOne = _players.First(p => p.Tickets.Any(t => t.Id == winningTicketOne.Id));
        var winnerTwo = _players.First(p => p.Tickets.Any(t => t.Id == winningTicketTwo.Id));
        Assert.Equivalent(new[] { winnerOne.Id, winnerTwo.Id }, result.SecondTier.Winners.Select(p => p.Id));
    }
    
    [Fact]
    public void The_second_tier_prize_will_be_30_percent_of_the_ticket_revenue_split_between_two()
    {
        // Act
        var result = _game.Run(_playerRepo, Seed);
        
        // Assert
        Assert.Equal(new Amount("USD", 3), result.SecondTier.Value);
    }
}