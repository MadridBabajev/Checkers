using Domain.Db;

namespace DAL.Db;

public class GameOptionsRepository : IGameOptionsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public GameOptionsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string Name => "DB - SQLite";
    
    public List<CheckersOptions> GetCheckersOptionsList()
    {
        throw new NotImplementedException();
    }

    public CheckersOptions GetCheckersOptionsById(string id)
    {
        throw new NotImplementedException();
    }

    public void SaveCheckersOptions(string id, CheckersOptions options)
    {
        throw new NotImplementedException();
    }

    public void DeleteCheckersOptions(string id)
    {
        throw new NotImplementedException();
    }
    
}