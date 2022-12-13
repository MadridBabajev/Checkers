namespace Move;

public class Move
{
    public int XFrom { get; }
    public int YFrom { get; }
    public int XTo { get; }
    public int YTo { get; }

    public Move(int xFrom, int yFrom, int xTo, int yTo)
    {
        XFrom = xFrom;
        YFrom = yFrom;
        XTo = xTo;
        YTo = yTo;
    }
}