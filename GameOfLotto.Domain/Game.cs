namespace GameOfLotto.Domain;

public class Game(Guid id = default)
{
    public Guid Id { get; private set; } = id;

    public Result Run(IPlayerRepository playerRepository, int seed)
    {
        var players = playerRepository.Get().Where(p => p.GameIds.Contains(Id)).ToArray();
        var tickets = players.SelectMany(p => p.Tickets).ToArray();
        var rng = new Random(seed);
        var grandPrize = rng.Next(1, tickets.Length);
        var winningTicket = tickets.ElementAt(grandPrize - 1);
        var grandPrizeWinner = players.First(p => p.Tickets.Contains(winningTicket));
        return new Result(new Prize([grandPrizeWinner], new Amount("USD", 0)));
    }

    public IList<object> GetPlayers()
    {
        return [];
    }

    public record Result(Prize GrandPrize);

    public record Prize(Player[] Winners, Amount Value);
}
