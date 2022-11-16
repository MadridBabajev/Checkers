
namespace Domain.Db;

public class CheckersGame
{
    // TODO Add initial pieces amount, current white and black pieces
    public int Id { get; set; }

    public DateTime StartedAt { get; set; } = DateTime.Now;
    public DateTime? GameOverAt { get; set; }
    public EGameType GameType { get; set; } = EGameType.Local;
    public Player? GameWonByPlayer { get; set; }

    public int GamePlayer1Id { get; set; }
    public Player? GamePlayer1 { get; set; }
    
    public int GamePlayer2Id { get; set; }
    public Player? GamePlayer2 { get; set; }

    public int CheckersOptionsId { get; set; }
    public CheckersOptions? CheckersOptions { get; set; }
    public ICollection<GameState>? GameStates { get; set; }
}
