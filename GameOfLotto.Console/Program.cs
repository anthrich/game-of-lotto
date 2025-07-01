// See https://aka.ms/new-console-template for more information

using GameOfLotto;
using GameOfLotto.Domain;

var game = new Game();
var playerRepo = new InMemoryPlayerRepository();
var playerManifest = new PlayerManifest(playerRepo);
var ticketOffice = new TicketOffice(playerRepo);
var player = playerManifest.AddPlayer("Player 1", game);

Console.WriteLine($"Welcome to the GameOfLotto, {player}!");
Console.WriteLine();
Console.WriteLine($"How many tickets do you want to buy, {player}?");

var ticketCount = Convert.ToUInt32(Console.ReadLine());

ticketOffice.Purchase(player.Id, ticketCount);

player = playerRepo.GetById(player.Id);

Console.WriteLine($"{player} has {player.Tickets.Count} tickets");

Console.WriteLine();

