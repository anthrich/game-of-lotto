namespace GameOfLotto.Domain.Tests;

public class WhenAddingCpuPlayers
{
    private readonly InMemoryPlayerRepository _playerRepository;
    private readonly PlayerManifest _playerManifest;
    private readonly Game _game = new(Guid.NewGuid());

    public WhenAddingCpuPlayers()
    {
        _playerRepository = new InMemoryPlayerRepository();
        _playerManifest = new PlayerManifest(_playerRepository, _game);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(15)]
    public void It_adds_the_correct_number_of_players(int numberOfPlayers)
    {
        // Act
        _playerManifest.AddCpuPlayers("Player", numberOfPlayers);
        
        // Assert
        var players = _playerRepository.Get();
        Assert.Equal(numberOfPlayers, players.Count);
    }
}