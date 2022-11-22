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
    Task<List<GameState>> GetGameStates();
    GameState? GetGameLastState();

    CheckersOptions GetOptionsById(int gameOptionsFk);
    
    Player GetPlayerById(int gamePlayerFk);

    // create and update
    void SaveGame(string id, CheckersGame game);
    void AddState(string currentState, int? id);
    
    // delete
    void DeleteGame(string id);
}
