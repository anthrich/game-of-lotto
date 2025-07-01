namespace GameOfLotto.Domain;

public class Player(string name, Guid id = default)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; }= name;
    public List<Guid> GameIds { get; private set; }= [];
    public List<Ticket> Tickets { get; private set; } = [];

    internal void AddGame(Guid gameId)
    {
        GameIds.Add(gameId);
    }

    internal void AddTicket(Ticket ticket)
    {
        Tickets.Add(ticket);
    }

    public override string ToString()
    {
        return Name;
    }
};