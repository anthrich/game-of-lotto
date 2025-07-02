namespace GameOfLotto.Domain;

public class TicketOffice(IPlayerRepository repository)
{
    public void Purchase(Guid playerId, uint numberOfTickets)
    {
        var player = repository.GetById(playerId);
        
        for (var i = 0; i < numberOfTickets; i++)
        {
            player.AddTicket(new Ticket(Guid.NewGuid()));
        }
        
        repository.Save(player);
    }
}