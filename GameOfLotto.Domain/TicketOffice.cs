namespace GameOfLotto.Domain;

public class TicketOffice(IPlayerRepository repository)
{
    public void Purchase(Guid playerId, uint numberOfTickets)
    {
        var player = repository.GetById(playerId);
        player.AddTicket(new Ticket(Guid.NewGuid()));
        repository.Save(player);
    }
}