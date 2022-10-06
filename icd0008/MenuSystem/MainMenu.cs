using GameOptions;

namespace MenuSystem;

public class MainMenu : IMenu
{
    private static readonly Dictionary<string, MenuItem> MainMenuItems = new()
    {
        {"N", new MenuItem("New Game", "N", new NewGameMenu())},
        {"L", new MenuItem("Load Game", "L", new LoadGameMenu())},
        {"O", new MenuItem("Options", "O", new OptionsMenu())},
        {"E", new MenuItem("Exit", "E", null)}
    };
    public void InitialiseMenu()
    {
        Console.WriteLine("== Welcome to Checkers game! ==");
        // bool userExited = false;
        while (true)
        {
            // Console.WriteLine("[Currently in the main menu..]");
            Console.WriteLine(((IMenu)this).InitialMenu());
            Console.Write("Your choice: ");
            var userInput = Console.ReadLine()?.ToUpper().Trim();
            switch (userInput)
            {
                case "N":
                case "L":
                case "O":
                    ((IMenu)this).RedirectTo(userInput);
                    break;
                case "E":
                    return;
                default:
                    Console.Write("Invalid input! Choose one of those [N, L, O, E]\n");
                    continue;
            }
        }
    }

    string IMenu.InitialMenu()
    {
        var retString = MainMenuItems.Values.Aggregate("==================\n",
            (current, menu) => current + menu + "\n");
        retString += "==================";
        return retString;
    }

    void IMenu.RedirectTo(string userInput)
    {
        switch (userInput)
        {
            case "N":
                var newGameMenu = MainMenuItems["N"];
                newGameMenu.GetMenu()?.InitialiseMenu();
                break;
            case "L":
                var loadGameMenu = MainMenuItems["L"];
                loadGameMenu.GetMenu()?.InitialiseMenu();
                break;
            case "O":
                var optionsMenu = MainMenuItems["O"];
                optionsMenu.GetMenu()?.InitialiseMenu();
                break;
        }
    }

    private static void Main() {}
    public void InitialiseMenu(Options options) {}

}