using GameOptions;
using GameParts;

namespace Domain;

public class Game
{
    public string? GameId { get; set; }
    public Options? GameOptions { get; set; }
    public EGameType GameType { get; set; }
    public List<CheckersPiece>? BoardState { get; set; }
    public string? SavedDate { get; set; }
    
    public override string ToString()
    {
        return $"{GameId}\n" +
               $"{GameOptions}\n" +
               $"{GameType}\n" +
               $"{BoardState}" +
               $"{SavedDate}\n";
    }
    private static void Main() {}
}


    
