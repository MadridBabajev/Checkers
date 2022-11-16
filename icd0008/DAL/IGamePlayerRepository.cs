using Domain.Db;

namespace DAL;

public interface IGamePlayerRepository
{
    // Set different names in case you use various DB engines
    string Name { get; }
    
    // CRUD methods
    
    // read
    Task<List<Player>> GetPlayerList();
    Player GetPlayerById(string id);
    
    // create and update
    void SavePlayer(string id, Player player);
    
    // delete
    void DeletePlayer(string id);
}