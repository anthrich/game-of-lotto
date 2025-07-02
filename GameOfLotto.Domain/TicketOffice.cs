namespace GameOfLotto.Domain;

public class TicketOffice(IPlayerRepository repository)
{
    public void Purchase(Guid playerId, uint numberOfTickets)
    {
        var player = repository.GetById(playerId);
        
        for (var i = 0; i < numberOfTickets; i++)
        {
            var ticketWasBought = player.BuyTicket(new Ticket(Guid.NewGuid(), new Amount("USD", 1)));
            if(!ticketWasBought) break;
        }
        
        repository.Save(player);
    }
}