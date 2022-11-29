namespace Domain.Db;

public class CurrentGameState
{
    public List<CheckersPiece> GameBoard { get; set; } = default!;
    public bool CurrentMoveByWhite { get; set; }

    public short WhitesLeft { get; set; }
    public short BlacksLeft { get; set; }
    public static void Main() {}
}
