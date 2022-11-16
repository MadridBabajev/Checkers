namespace Domain.Db;

public class CheckersOptions
{
    
    public int Id { get; set; }
    
    public string Name { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool WhitesFirst { get; set; }

    public bool MandatoryTake { get; set; }

    public bool QueensHaveOpMoves { get; set; }

    public short BoardWidth { get; set; }

    public short BoardHeight { get; set; }
    
    public ICollection<CheckersGame>? OthelloGames { get; set; }
    
    public override string ToString() => 
        $"Whites First -> {WhitesFirst}\n" +
        $"Mandatory Take -> {MandatoryTake}\n" +
        $"Queens Have OP Moves -> {QueensHaveOpMoves}\n" +
        $"Board Width -> {BoardWidth}\n" +
        $"Board Height -> {BoardHeight}";
}