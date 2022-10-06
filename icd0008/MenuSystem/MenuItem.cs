namespace MenuSystem;
public class MenuItem
{
    private string Title { get;}
    private string Shortcut { get;}
    private IMenu? Menu { get; }

    public MenuItem(string title, string shortcut, IMenu? menu)
    {
        Title = title;
        Shortcut = shortcut;
        Menu = menu;
    }

    public IMenu? GetMenu()
    {
        return Menu;
    }

    public override string ToString() =>
        $"{Shortcut}) {Title}";
}