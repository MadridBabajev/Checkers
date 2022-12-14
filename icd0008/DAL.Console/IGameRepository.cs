using Domain;
using GameOptions;
using GameParts;

namespace DAL;

public interface IGameRepository
{
    // crud methods
    
    // Read
    public static List<Game>? GetAllGamesList() {return null;}
    public static Game? GetGameById(string id){return null;}

    // Create and update
    public static void SaveGame(string id, Options options, EGameType gameType, List<CheckersPiece> boardState) {}

    // Delete
    public static void DeleteGame(string id){}
    
    
}