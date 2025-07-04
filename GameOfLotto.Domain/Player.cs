﻿namespace GameOfLotto.Domain;

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

    internal bool BuyTicket(Ticket ticket)
    {
        if (Balance >= ticket.Cost)
        {
            Tickets.Add(ticket);
            Balance -= ticket.Cost;
            return true;
        }
        
        return false;
    }

    public override string ToString()
    {
        return $"{Name} has {Tickets.Count} tickets, and a balance of {Balance}";
    }
};