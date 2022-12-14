namespace GameParts;

public class CheckersPiece
{
    private short _yCoordinate ;
    private short _xCoordinate;

    public CheckersPiece(EPieceColor color, short yCoordinate, short xCoordinate, bool isQueen)
    {
        Color = color;
        _yCoordinate = yCoordinate;
        _xCoordinate = xCoordinate;
        IsQueen = isQueen;
    }

    public EPieceColor Color { get; }

    public short XCoordinate
    {
        get => _xCoordinate;
        set => _xCoordinate = value;
    }

    public short YCoordinate
    {
        get => _yCoordinate;
        set => _yCoordinate = value;
    }

    public bool IsQueen { get; set; }

    public override string ToString()
    {
        return $"Color -> {Color}; " +
               $"Y -> {YCoordinate}; " +
               $"X -> {XCoordinate}; " +
               $"isQueen -> {IsQueen}; ";
    }
    
}