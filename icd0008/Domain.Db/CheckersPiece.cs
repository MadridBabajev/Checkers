namespace Domain.Db;

public class CheckersPiece
{
    public short YCoordinate { get; set; }
    public short XCoordinate { get; set; }
    public EPieceColor Color { get; set; }
    public bool IsQueen { get; set; }
    public CheckersPiece(short x, short y, EPieceColor color)
    {
        XCoordinate = x;
        YCoordinate = y;
        Color = color;
    }
    
    public CheckersPiece(short x, short y, EPieceColor color, bool isQueen)
    {
        XCoordinate = x;
        YCoordinate = y;
        Color = color;
        IsQueen = isQueen;
    }
    
    public CheckersPiece() {}
    public override string ToString()
    {
        return $"{Color.ToString()} piece at {XCoordinate}-{YCoordinate}";
    }
}
