namespace GameOfLotto.Domain;

public class Player(string name, Amount balance, Guid id = default)
{
    public Guid Id { get; set; } = id;
    public string Name { get; private set; } = name;
    public List<Guid> GameIds { get; private set; } = [];
    public List<Ticket> Tickets { get; private set; } = [];
    public Amount Balance { get; private set; } = balance;

    internal void AddGame(Guid gameId)
    {
        GameIds.Add(gameId);
    }

    internal void AddTicket(Ticket ticket)
    {
        Tickets.Add(ticket);
        Balance -= ticket.Cost;
    }

    public override string ToString()
    {
        return Name;
    }
};