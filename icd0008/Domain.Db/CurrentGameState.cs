namespace Domain.Db;

public class CurrentGameState
{
    public List<CheckersPiece> GameBoard = default!;
    public bool NextMoveByWhite { get; set; }

    public static void Main() {}
}