namespace GameParts;

public class SavedDataFromUi
{
    public SavedDataFromUi(List<CheckersPiece> checkersPieces, List<string?> widthSpecifiers, List<string?> heightSpecifiers)
    {
        CheckersPieces = checkersPieces;
        WidthSpecifiers = widthSpecifiers;
        HeightSpecifiers = heightSpecifiers;
    }

    public List<CheckersPiece> CheckersPieces { get; }
    public List<string?> WidthSpecifiers { get; }
    public List<string?> HeightSpecifiers { get; }
}