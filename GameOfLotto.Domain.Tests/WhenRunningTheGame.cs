namespace GameOfLotto.Domain.Tests;

public class WhenRunningTheGame
{
    private readonly Game _game;
    private readonly InMemoryPlayerRepository _playerRepo;
    private readonly Player[] _players;
    private Ticket[] _tickets;
    private const int Seed = 1234;
    private readonly int _winningTicketNumber;
    private readonly int[] _secondTierWinningTicketNumbers = [0, 0];
    private readonly int[] _thirdTierWinningTicketNumbers = [0, 0, 0, 0];

    public WhenRunningTheGame()
    {
        _game = new Game();
        _playerRepo = new InMemoryPlayerRepository();
        var playerManifest = new PlayerManifest(_playerRepo, _game);
        var ticketOffice = new TicketOffice(_playerRepo);
        _players = playerManifest.AddCpuPlayers("Player", 10).ToArray();
        
        foreach (var player in _players)
        {
            ticketOffice.Purchase(player.Id, 2);
        }

        _players = _playerRepo.Get().ToArray();
        _tickets = _players.SelectMany(p => p.Tickets).ToArray();
        var random = new Random(Seed);
        _winningTicketNumber = random.Next(1, 20);
        _secondTierWinningTicketNumbers[0] = random.Next(1, 19);
        _secondTierWinningTicketNumbers[1] = random.Next(1, 18);
        _thirdTierWinningTicketNumbers[0] = random.Next(1, 17);
        _thirdTierWinningTicketNumbers[1] = random.Next(1, 16);
        _thirdTierWinningTicketNumbers[2] = random.Next(1, 15);
        _thirdTierWinningTicketNumbers[3] = random.Next(1, 14);
    }

    [Fact]
    public void The_first_ticket_drawn_will_award_the_grand_prize_to_its_owner()
    {
        // Act
        var result = _game.Run(_playerRepo, Seed);
        
        // Assert
        var winner = GetGrandPrizeWinner();
        Assert.Equivalent(new[] { winner.Item1 }, result.GrandPrize.Winners);
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
        var winners = GetSecondTierWinners();
        Assert.Equivalent(winners.Select(w => w.Key), result.SecondTier.Winners);
    }

    [Fact]
    public void The_second_tier_prize_will_be_30_percent_of_the_ticket_revenue_split_between_two()
    {
        // Act
        var result = _game.Run(_playerRepo, Seed);
        
        // Assert
        Assert.Equal(new Amount("USD", 3), result.SecondTier.Value);
    }

    [Fact]
    public void The_next_4_tickets_will_be_awarded_the_third_tier_prizes()
    {
        
        // Act
        var result = _game.Run(_playerRepo, Seed);
        
        // Assert
        var winners = GetThirdTierWinners();
        Assert.Equivalent(winners.Select(w => w.Key), result.ThirdTier.Winners);
    }

    private (Player, Ticket) GetGrandPrizeWinner()
    {
        var winningTicket = _tickets[_winningTicketNumber - 1];
        var winner = _players.First(p => p.Tickets.Any(t => t.Id == winningTicket.Id));
        return (winner, winningTicket);
    }
    
    private KeyValuePair<Player, Ticket>[] GetSecondTierWinners()
    {
        var grandPrizeWinner = GetGrandPrizeWinner();
        _tickets = _tickets.Except([grandPrizeWinner.Item2]).ToArray();
        var winningTicketOne = _tickets[_secondTierWinningTicketNumbers[0] - 1];
        _tickets = _tickets.Except([winningTicketOne]).ToArray();
        var winningTicketTwo = _tickets[_secondTierWinningTicketNumbers[1] - 1];
        var winnerOne = _players.First(p => p.Tickets.Any(t => t.Id == winningTicketOne.Id));
        var winnerTwo = _players.First(p => p.Tickets.Any(t => t.Id == winningTicketTwo.Id));
        return [
            new KeyValuePair<Player, Ticket>(winnerOne, winningTicketOne),
            new KeyValuePair<Player, Ticket>(winnerTwo, winningTicketTwo)
        ];
    }
    
    private KeyValuePair<Player, Ticket>[] GetThirdTierWinners()
    {
        var secondTierWinners = GetSecondTierWinners();
        _tickets = _tickets.Except(secondTierWinners.Select(w => w.Value)).ToArray();
        var winningTicket1 = _tickets[_thirdTierWinningTicketNumbers[0] - 1];
        _tickets = _tickets.Except([winningTicket1]).ToArray();
        var winningTicket2 = _tickets[_thirdTierWinningTicketNumbers[1] - 1];
        _tickets = _tickets.Except([winningTicket2]).ToArray();
        var winningTicket3 = _tickets[_thirdTierWinningTicketNumbers[2] - 1];
        _tickets = _tickets.Except([winningTicket3]).ToArray();
        var winningTicket4 = _tickets[_thirdTierWinningTicketNumbers[3] - 1];
        var winner1 = _players.First(p => p.Tickets.Any(t => t.Id == winningTicket1.Id));
        var winner2 = _players.First(p => p.Tickets.Any(t => t.Id == winningTicket2.Id));
        var winner3 = _players.First(p => p.Tickets.Any(t => t.Id == winningTicket3.Id));
        var winner4 = _players.First(p => p.Tickets.Any(t => t.Id == winningTicket4.Id));
        return [
            new KeyValuePair<Player, Ticket>(winner1, winningTicket1),
            new KeyValuePair<Player, Ticket>(winner2, winningTicket2),
            new KeyValuePair<Player, Ticket>(winner3, winningTicket3),
            new KeyValuePair<Player, Ticket>(winner4, winningTicket4)
        ];
    }
}