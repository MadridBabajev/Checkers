using GameOptions;
using GameParts;

namespace Domain;

public class Game
{
    public string? GameId { get; set; }
    public Options? GameOptions { get; set; }
    public EGameType GameType { get; set; }
    public List<CheckersPiece>? BoardState { get; set; }
    public List<string?>? HeightSpecifiers { get; set; }
    public List<string?>? WidthSpecifiers { get; set; }
    public bool? WhitesTurn { get; set; }
    public string? SavedDate { get; set; }
    
    public override string ToString()
    {
        return $"Game id -> {GameId}\n";
    }
    
}
