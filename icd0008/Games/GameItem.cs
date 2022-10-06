namespace Games;

public class GameItem
{
    private string Title { get; }
    private string ShortCut { get; }
    private IGame? Game { get; }

    public GameItem(string title, string shortCut, IGame? game)
    {
        Title = title;
        ShortCut = shortCut;
        Game = game;
    }

    public IGame? GetGame()
    {
        return Game;
    }

    public override string ToString() => 
        $"{ShortCut}) {Title}";
    
}