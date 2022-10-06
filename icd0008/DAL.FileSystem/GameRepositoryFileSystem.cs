using System.Globalization;
using System.Text.Json;
using System.Web;
using Domain;
using GameOptions;
using GameParts;

namespace DAL.FileSystem;

public class GameRepositoryFileSystem : IGameRepository
{
    private const string FileExtension = "json";
    private static readonly string DirectoryLocation = @"." + Path.DirectorySeparatorChar + "SavedGame";
    private static readonly Game LastSavedGame = new();

    public static List<Game> GetAllGamesList()
    {
        var retList = new List<Game>();
        foreach (var fileName in Directory.GetFileSystemEntries(DirectoryLocation, "*." + FileExtension))
        {
            retList.Add(GetGameById(fileName));
        }
        return retList;
    }

    public static Game GetGameById(string id)
    {
        var fileContent = File.ReadAllText(GetFileName(id));
        var game = JsonSerializer.Deserialize<Game>(fileContent);
        if (game == null) throw new NullReferenceException($"Could not deserialize: {fileContent}");
        return game;
    }

    public static void SaveGame(string id, Options? options, EGameType gameType, List<CheckersPiece>? boardState, DateTime savedDate)
    {
        if (options != null && boardState != null)
        {
            if (!Directory.Exists(DirectoryLocation)) Directory.CreateDirectory(DirectoryLocation);
            SetCurrentGameProperties(id, options, gameType, boardState, savedDate);
            var jsonString = JsonSerializer.Serialize(LastSavedGame);
            
            File.WriteAllText(GetFileName(id), jsonString);
            Console.WriteLine("Game saved!\n");
        }
    }
    
    public static void DeleteGame(string id)
    {
        File.Delete(GetFileName(id));
    }
    private static string GetFileName(string id)
    {
        return DirectoryLocation +
               Path.DirectorySeparatorChar +
               HttpUtility.UrlEncode(id, System.Text.Encoding.UTF8)
               + "." + FileExtension;
    }
    private static void SetCurrentGameProperties(string id, Options options, EGameType gameType, List<CheckersPiece>? boardState, DateTime savedDate)
    {
        LastSavedGame.GameId = id;
        LastSavedGame.GameOptions = options;
        LastSavedGame.GameType = gameType;
        LastSavedGame.BoardState = boardState;
            LastSavedGame.SavedDate = savedDate.ToString(CultureInfo.CurrentCulture);
    }
    
    private static void Main() {}
}