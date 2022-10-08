
namespace MenuSystem;

public class MainMenu : IMenu
{
    private readonly string[] _mainMenuItems = { "New Game", "Load Game", "DeleteGame", "Options", "Exit" };

    private static readonly Dictionary<int, IMenu> MainMenuItemsDictionary = new()
    {
        { 0, new NewGameMenu() },
        { 1, new LoadGameMenu() },
        { 2, new DeleteGameMenu() },
        { 3, new OptionsMenu() }
    };

    public void InitialiseMenu()
    {
        bool userWantsToExist = false;
        while (!userWantsToExist)
        {
            int userChoice = ConsoleHelper.MultipleChoice(true, _mainMenuItems);
            switch (userChoice)
            {
                case -1:
                case 4 :    
                    Console.WriteLine("\nExiting The game..");
                    userWantsToExist = true;
                    break;
                case 0:
                case 1:
                case 2:
                case 3:
                    MainMenuItemsDictionary[userChoice].InitialiseMenu();
                    break;
            }
        }
    }
    private static void Main() {}
    
}