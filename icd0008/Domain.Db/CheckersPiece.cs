namespace Domain.Db;

public class CheckersPiece
{
    public short YCoordinate { get; set; }
    public short XCoordinate { get; set; }
    public EPieceColor Color { get; set; }
    public bool IsQueen { get; set; } = false;
    public CheckersPiece(short x, short y, EPieceColor color)
    {
        XCoordinate = x;
        YCoordinate = y;
        Color = color;
    }
    
    public CheckersPiece() {}
    public override string ToString()
    {
        return $"{Color.ToString()} piece at {XCoordinate}-{YCoordinate}";
    }
}
