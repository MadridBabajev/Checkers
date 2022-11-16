namespace Domain.Db;

public class CheckersPiece
{
    public short YCoordinate { get; set; } = default!;
    public short XCoordinate { get; set; } = default!;
    public EPieceColor Color { get; set; } = default!;
    public bool IsQueen { get; set; } = false;
}
