﻿using GameOfLotto;
using GameOfLotto.Domain;

var game = new Game();
var playerRepo = new InMemoryPlayerRepository();
var playerManifest = new PlayerManifest(playerRepo, game);
var ticketOffice = new TicketOffice(playerRepo);

var humanPlayer = playerManifest.AddPlayer("Player 1");
var random = new Random();

Console.WriteLine($"Welcome to the GameOfLotto, {humanPlayer}!");
Console.WriteLine();
Console.WriteLine($"How many tickets do you want to buy, {humanPlayer.Name}?");

var ticketCount = Convert.ToUInt32(Console.ReadLine());

ticketOffice.Purchase(humanPlayer.Id, ticketCount);

var cpuPlayers = playerManifest.AddCpuPlayers("CPU", random.Next(9, 14));

foreach (var cpuPlayer in cpuPlayers)
{
    ticketOffice.Purchase(cpuPlayer.Id, (uint)random.Next(1, 10));
}

var players = playerRepo.Get();

Console.WriteLine();
Console.WriteLine("Players:");
Console.WriteLine();

foreach (var player in players)
{
    Console.WriteLine(player.ToString());
}

var result = game.Run(playerRepo, random.Next());

Console.WriteLine();
Console.WriteLine("Ticket draw results:");
Console.WriteLine();
Console.WriteLine($"* Grand Prize: {result.GrandPrize}");
Console.WriteLine($"* Second Tier: {result.SecondTier}");
Console.WriteLine($"* Third Tier: {result.ThirdTier}");
Console.WriteLine();
Console.WriteLine("Congratulations to the winners!");
Console.WriteLine();
Console.WriteLine($"House Revenue: {result.HouseRevenue}");
Console.WriteLine();
Console.WriteLine("Press any key to continue...");
Console.ReadKey();

