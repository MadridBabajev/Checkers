using Domain.Db;

namespace DAL;

public interface IGameRepository
{
    // Set different names in case you use various DB engines
    string Name { get; }
    
    // CRUD methods
    
    // read
    Task<List<CheckersGame>> GetGamesList();
    CheckersGame? GetGameById(string id);
    
    // create and update
    void SaveGame(string id, CheckersGame game);
    
    // delete
    void DeleteGame(string id);
}
