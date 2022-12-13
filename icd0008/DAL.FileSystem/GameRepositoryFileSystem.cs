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
        return Directory.GetFileSystemEntries
            (DirectoryLocation, "*." + FileExtension)
            .Select(fileName => GetGameById(fileName)).ToList();
    }

    public static Game GetGameById(string id)
    {
        var fileContent = File.ReadAllText(id);
        var game = JsonSerializer.Deserialize<Game>(fileContent);
        if (game == null) throw new NullReferenceException($"Could not deserialize: {fileContent}");
        return game;
    }

    public static void SaveGame(string id, Options? options,
        EGameType gameType, List<CheckersPiece>? boardState, List<string?>? heightSpecifiers,
        List<string?>? widthSpecifiers, bool whitesTurn, DateTime savedDate)
    {
        if (options != null && boardState != null)
        {
            if (!Directory.Exists(DirectoryLocation)) Directory.CreateDirectory(DirectoryLocation);
            SetCurrentGameProperties(id, options, gameType,
                boardState, heightSpecifiers, widthSpecifiers, whitesTurn, savedDate);
            var jsonString = JsonSerializer.Serialize(LastSavedGame);
            
            File.WriteAllText(GetFileName(id), jsonString);
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
    private static void SetCurrentGameProperties(string id, Options options, EGameType gameType,
        List<CheckersPiece>? boardState,List<string?>? heightSpecifiers,
        List<string?>? widthSpecifiers, bool whitesTurn, DateTime savedDate)
    {
        LastSavedGame.GameId = id;
        LastSavedGame.GameOptions = options;
        LastSavedGame.GameType = gameType;
        LastSavedGame.BoardState = boardState;
        LastSavedGame.HeightSpecifiers = heightSpecifiers;
        LastSavedGame.WidthSpecifiers = widthSpecifiers;
        LastSavedGame.WhitesTurn = whitesTurn;
        LastSavedGame.SavedDate = savedDate.ToString(CultureInfo.CurrentCulture);
    }
    
    private static void Main() {}
}