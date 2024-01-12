using System;
using System.Collections.Generic;

// Interfaces
public interface IPlayerRepository
{
    void CreatePlayer(PlayerEntity player);
    List<PlayerEntity> ReadAllPlayers();
}

public interface IGameRepository
{
    void CreateGame(GameEntity game);
    List<GameEntity> ReadAllGames();
    List<GameEntity> ReadPlayerGamesByPlayerId(int playerId);
}

// Entities
public class PlayerEntity
{
    public int PlayerId { get; set; }
    public string UserName { get; set; }
    public int CurrentRating { get; set; }
}

public class GameEntity
{
    public int GameId { get; set; }
    public int PlayerId { get; set; }
    public string OpponentName { get; set; }
    public string Outcome { get; set; }
    public int Rating { get; set; }
}

// DbContext
public class MyDbContext : IPlayerRepository, IGameRepository
{
    private List<PlayerEntity> players;
    private List<GameEntity> games;

    public MyDbContext()
    {
        players = new List<PlayerEntity>();
        games = new List<GameEntity>();
    }

    public void CreatePlayer(PlayerEntity player)
    {
        player.PlayerId = players.Count + 1;
        players.Add(player);
    }

    public List<PlayerEntity> ReadAllPlayers()
    {
        return players;
    }

    public void CreateGame(GameEntity game)
    {
        game.GameId = games.Count + 1;
        games.Add(game);
    }

    public List<GameEntity> ReadAllGames()
    {
        return games;
    }

    public List<GameEntity> ReadPlayerGamesByPlayerId(int playerId)
    {
        return games.FindAll(g => g.PlayerId == playerId);
    }
}

// Service
public class GameService
{
    private IPlayerRepository playerRepository;
    private IGameRepository gameRepository;

    public GameService(IPlayerRepository playerRepository, IGameRepository gameRepository)
    {
        this.playerRepository = playerRepository;
        this.gameRepository = gameRepository;
    }

    public void CreatePlayerAndGame(string playerName, int initialRating, string opponentName, string outcome, int rating)
    {
        PlayerEntity player = new PlayerEntity { UserName = playerName, CurrentRating = initialRating };
        playerRepository.CreatePlayer(player);

        GameEntity game = new GameEntity { PlayerId = player.PlayerId, OpponentName = opponentName, Outcome = outcome, Rating = rating };
        gameRepository.CreateGame(game);
    }

    public void DisplayAllPlayers()
    {
        List<PlayerEntity> players = playerRepository.ReadAllPlayers();
        Console.WriteLine("All Players:");
        foreach (var player in players)
        {
            Console.WriteLine($"Player ID: {player.PlayerId}, Username: {player.UserName}, Current Rating: {player.CurrentRating}");
        }
        Console.WriteLine();
    }

    public void DisplayAllGames()
    {
        List<GameEntity> games = gameRepository.ReadAllGames();
        Console.WriteLine("All Games:");
        foreach (var game in games)
        {
            Console.WriteLine($"Game ID: {game.GameId}, Player ID: {game.PlayerId}, Opponent: {game.OpponentName}, Outcome: {game.Outcome}, Rating: {game.Rating}");
        }
        Console.WriteLine();
    }

    public void DisplayPlayerGames(int playerId)
    {
        List<GameEntity> playerGames = gameRepository.ReadPlayerGamesByPlayerId(playerId);
        Console.WriteLine($"Games for Player ID {playerId}:");
        foreach (var game in playerGames)
        {
            Console.WriteLine($"Game ID: {game.GameId}, Opponent: {game.OpponentName}, Outcome: {game.Outcome}, Rating: {game.Rating}");
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        MyDbContext dbContext = new MyDbContext();
        GameService gameService = new GameService(dbContext, dbContext);

        // Simulate creating players and games
        gameService.CreatePlayerAndGame("Player1", 1000, "Opponent1", "Win", 20);
        gameService.CreatePlayerAndGame("Player2", 1200, "Opponent2", "Loss", 15);

        // Display all players and games
        gameService.DisplayAllPlayers();
        gameService.DisplayAllGames();

        // Display games for a specific player
        gameService.DisplayPlayerGames(1);
    }
}
