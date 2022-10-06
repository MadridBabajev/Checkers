
namespace MenuSystem;

public interface IMenu
{
    

    // // private static readonly List<MenuItem> MainMenuItems = new();
    public void InitialiseMenu();
    void RedirectTo(string? userInput);
    string InitialMenu();
}