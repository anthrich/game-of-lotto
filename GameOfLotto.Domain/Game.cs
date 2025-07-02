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
        var secondTierCohort = Math.Round(allTickets.Count * 0.1m);
        var ticketsInPlay = allTickets.Except([grandPrize.Ticket]).ToList();
        var secondTierWinners = RunTier(secondTierCohort, rng, ticketsInPlay, players);
        var thirdTierCohort = Math.Round(allTickets.Count * 0.2m);
        var thirdTierWinners = RunTier(thirdTierCohort, rng, ticketsInPlay, players);
        var totalRevenue = allTickets.Sum(t => t.Cost.Value);
        var grandPrizeAmount = totalRevenue * 0.5m;
        var secondTierAmount = totalRevenue * 0.3m;
        var thirdTierAmount = totalRevenue * 0.1m;
        
        return new Result(
            new Prize([grandPrize.Player], new Amount("USD", grandPrizeAmount)),
            new Prize(secondTierWinners.ToArray(), new Amount("USD", secondTierAmount / secondTierCohort)),
            new Prize(thirdTierWinners.ToArray(), new Amount("USD", thirdTierAmount / thirdTierCohort)),
            new Amount("USD", totalRevenue - grandPrizeAmount - secondTierAmount - thirdTierAmount)
        );
    }

    private static List<Player> RunTier(decimal numberOfWinners, Random rng, List<Ticket> ticketsInPlay, Player[] players)
    {
        var secondTierWinners = new List<Player>();
        
        for (var i = 0; i < numberOfWinners; i++)
        {
            var prize = GetPrize(rng, ticketsInPlay, players);
            ticketsInPlay.Remove(prize.Ticket);
            secondTierWinners.Add(prize.Player);
        }
        
        return secondTierWinners;
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

    public record Result(Prize GrandPrize, Prize SecondTier, Prize ThirdTier, Amount HouseRevenue);

    public record Prize(Player[] Winners, Amount Value)
    {
        public override string ToString()
        {
            var winString = Winners.Length > 1 ? "win" : "wins";
            var eachString = Winners.Length > 1 ? " each" : "";
            return $"{string.Join(", ", Winners.Select(w => w.Name))} {winString} {Value}{eachString}!";
        }
    };
    
    private record PrizeAward(Player Player, Ticket Ticket);
}
