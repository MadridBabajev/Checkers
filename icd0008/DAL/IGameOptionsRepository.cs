using Domain.Db;

namespace DAL;

public interface IGameOptionsRepository
{
    // Set different names in case you use various DB engines
    string Name { get; }
    
    // CRUD methods
    
    // read
    List<CheckersOptions> GetCheckersOptionsList();
    CheckersOptions GetCheckersOptionsById(string id);
    
    // create and update
    void SaveCheckersOptions(string id, CheckersOptions options);
    
    // delete
    void DeleteCheckersOptions(string id);
}