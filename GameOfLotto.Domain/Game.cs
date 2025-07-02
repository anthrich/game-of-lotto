namespace GameOfLotto.Domain;

public class Game(Guid id = default)
{
    public Guid Id { get; private set; } = id;

    public Result Run(IPlayerRepository playerRepository, int seed)
    {
        var players = playerRepository.Get().Where(p => p.GameIds.Contains(Id)).ToArray();
        var allTickets = players.SelectMany(p => p.Tickets).ToList();
        var rng = new Random(seed);
        var grandPrize = GetPrize(rng, allTickets, players);
        var grandPrizeAmount = allTickets.Sum(t => t.Cost.Value) * 0.5m;
        var secondTier = Math.Round(allTickets.Count * 0.1m);
        var ticketsInPlay = allTickets.Except([grandPrize.Ticket]).ToList();
        var secondTierWinners = new List<Player>();
        
        for (var i = 0; i < secondTier; i++)
        {
            var prize = GetPrize(rng, ticketsInPlay, players);
            ticketsInPlay.Remove(prize.Ticket);
            secondTierWinners.Add(prize.Player);
        }
        
        var secondTierAmount = allTickets.Sum(t => t.Cost.Value) * 0.3m / secondTier;
        
        return new Result(
            new Prize([grandPrize.Player], new Amount("USD", grandPrizeAmount)),
            new Prize(secondTierWinners.ToArray(), new Amount("USD", secondTierAmount))
        );
    }

    private static PrizeAward GetPrize(Random rng, List<Ticket> tickets, Player[] players)
    {
        var prize = rng.Next(1, tickets.Count);
        var prizeTicket = tickets.ElementAt(prize - 1);
        var prizeWinner = players.First(p => p.Tickets.Contains(prizeTicket));
        return new PrizeAward(prizeWinner, prizeTicket);
    }

    public IList<object> GetPlayers()
    {
        return [];
    }

    public record Result(Prize GrandPrize, Prize SecondTier);

    public record Prize(Player[] Winners, Amount Value);
    
    private record PrizeAward(Player Player, Ticket Ticket);
}
