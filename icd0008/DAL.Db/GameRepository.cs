using Domain.Db;
using Microsoft.EntityFrameworkCore;

namespace DAL.Db;

public class GameRepository : IGameRepository
{
    private readonly ApplicationDbContext _dbContext;

    public GameRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public string Name => "DB - SQLite";
    
    // .Include for join
    // .Select
    // .OrderBy
    public async Task<List<CheckersGame>> GetGamesList() 
        => await _dbContext.CheckersGames.ToListAsync();

    public CheckersGame? GetGameById(string id) => _dbContext.CheckersGames
        .First(g => g.Id == int.Parse(id));
    
    public async void SaveGame(string id, CheckersGame game)
    {
        var gameFromDb = _dbContext.CheckersGames
            .FirstOrDefault(g => g.Id == int.Parse(id));
        if (gameFromDb == null)
        {
            _dbContext.CheckersGames.Add(game);
            await _dbContext.SaveChangesAsync();
            return;
        }

        gameFromDb.StartedAt = game.StartedAt;
        gameFromDb.GameType = game.GameType;
        gameFromDb.GameWonByPlayer = game.GameWonByPlayer;
        gameFromDb.GamePlayer1 = game.GamePlayer1;
        gameFromDb.GamePlayer2 = game.GamePlayer2;
        gameFromDb.CheckersOptionsId = game.CheckersOptionsId;
        gameFromDb.CheckersOptions = game.CheckersOptions;
        gameFromDb.GameStates= game.GameStates;

        await _dbContext.SaveChangesAsync();
    }

    public async void DeleteGame(string id)
    {
        var gameFromDb = GetGameById(id);
        _dbContext.CheckersGames.Remove(gameFromDb!);
        await _dbContext.SaveChangesAsync();
    }
}