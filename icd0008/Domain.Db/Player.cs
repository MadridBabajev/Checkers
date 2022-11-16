using System.ComponentModel.DataAnnotations;

namespace Domain.Db;

public class Player
{
    public int Id { get; set; }
    [MaxLength(128)]
    [MinLength(2)]
    public string PlayerName { get; set; } = default!;

    public EPlayerType PlayerType { get; set; } = EPlayerType.Human;
    
    public ICollection<CheckersGame>? GamesAsPlayedP1 { get; set; }
    public ICollection<CheckersGame>? GamesAsPlayedP2 { get; set; }

    // [InverseProperty("Player2")]
    // public ICollection<CheckersGame>? GamesAsPlayer2 { get; set; }
}
