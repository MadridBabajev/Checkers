namespace GameOptions;

public class Options
{
    public Options(bool whitesFirst, bool mandatoryTake, bool queensHaveOpMoves, short boardWidth, short boardHeight)
    {
        WhitesFirst = whitesFirst;
        MandatoryTake = mandatoryTake;
        QueensHaveOpMoves = queensHaveOpMoves;
        BoardWidth = boardWidth;
        BoardHeight = boardHeight;
    }

    public bool WhitesFirst { get; set; }

    public bool MandatoryTake { get; set; }

    public bool QueensHaveOpMoves { get; set; }

    public short BoardWidth { get; set; }

    public short BoardHeight { get; set; }

    public override string ToString() => 
        $"Whites First -> {WhitesFirst}\n" +
        $"Mandatory Take -> {MandatoryTake}\n" +
        $"Queens Have OP Moves -> {QueensHaveOpMoves}\n" +
        $"Board Width -> {BoardWidth}\n" +
        $"Board Height -> {BoardHeight}";
    
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (obj is not Options)
        {
            return false;
        }

        return WhitesFirst == ((Options)obj).WhitesFirst
               && MandatoryTake == ((Options)obj).MandatoryTake
               && QueensHaveOpMoves == ((Options)obj).QueensHaveOpMoves
               && BoardWidth == ((Options)obj).BoardWidth
               && BoardHeight == ((Options)obj).BoardHeight;
    }

    protected bool Equals(Options other)
    {
        return WhitesFirst == other.WhitesFirst && MandatoryTake == other.MandatoryTake && QueensHaveOpMoves == other.QueensHaveOpMoves && BoardWidth == other.BoardWidth && BoardHeight == other.BoardHeight;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(WhitesFirst, MandatoryTake, QueensHaveOpMoves, BoardWidth, BoardHeight);
    }

    // public override int GetHashCode()
    // {
    //     return base.GetHashCode();
    // }
    
}