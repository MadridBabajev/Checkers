namespace GameParts;

public class SavedDataFromUI
{
    public SavedDataFromUI(List<CheckersPiece> checkersPieces, List<string> widthSpecifiers, List<string> heightCpecifiers)
    {
        CheckersPieces = checkersPieces;
        WidthSpecifiers = widthSpecifiers;
        HeightCpecifiers = heightCpecifiers;
    }

    public List<CheckersPiece> CheckersPieces { get; }
    public List<string?> WidthSpecifiers { get; }
    public List<string?> HeightCpecifiers { get; }
}