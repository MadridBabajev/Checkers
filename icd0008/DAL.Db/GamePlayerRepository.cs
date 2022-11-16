using Domain.Db;
using Microsoft.EntityFrameworkCore;

namespace DAL.Db;

public class GamePlayerRepository: IGamePlayerRepository
{
    
    private readonly ApplicationDbContext _dbContext;

    public GamePlayerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public string Name => "DB - SQLite";

    public async Task<List<Player>> GetPlayerList() 
        => await _dbContext.Players.ToListAsync();
    
    public Player GetPlayerById(string id) => _dbContext.Players.First(
        p => p.Id == int.Parse(id));

    public async void SavePlayer(string id, Player player)
    {
        var playerFromDb = _dbContext.Players
            .FirstOrDefault(g => g.Id == int.Parse(id));
        if (playerFromDb == null)
        {
            _dbContext.Players.Add(player);
            await _dbContext.SaveChangesAsync();
            return;
        }

        playerFromDb.PlayerName = player.PlayerName;
        playerFromDb.PlayerType = player.PlayerType;

        await _dbContext.SaveChangesAsync();
    }

    public async void DeletePlayer(string id)
    {
        var playerFromDb = GetPlayerById(id);
        _dbContext.Players.Remove(playerFromDb!);
        await _dbContext.SaveChangesAsync();
    }
}