namespace GameOptions;

public class Options
{
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

    // public override int GetHashCode()
    // {
    //     return base.GetHashCode();
    // }

    private static void Main() {}
}