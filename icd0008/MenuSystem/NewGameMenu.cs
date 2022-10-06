using GameOptions;
using Games;

namespace MenuSystem;

public class NewGameMenu: IMenu
{
    private static readonly Dictionary<string, GameItem> NewGameMenuItems = new()
    {
        {"PVP", new GameItem("VS Player Local", "PVP", new GamePlayerVsPlayer())},
        {"PVE", new GameItem("VS PC", "PVE", new GamePlayerVsPc())},
        {"EVE", new GameItem("PC VS PC", "EVE", new GamePcVsPc())},
        {"PVP_Online", new GameItem("VS Player Online", "PVP_Online", new GamePlayerVsPlayerOnline())}
    };

    public void InitialiseMenu()
    {
        Console.WriteLine("\n== Select game mode ==");
        while (true)
        {
            Console.WriteLine(((IMenu)this).InitialMenu());
            // Console.WriteLine("[Currently in the new game menu..]");
            Console.Write("Your choice: ");
            var userInput = Console.ReadLine()?.ToUpper().Trim();
            switch (userInput)
            {
                case "PVP":
                case "PVE":
                case "EVE":
                case "PVP_Online":
                    ((IMenu)this).RedirectTo(userInput);
                    break;
                case "B":
                    return;
                default:
                    Console.Write("Invalid input! Choose one of those: " +
                                  "\n[PVP, PVE, EVE, PVP_Online, B]\n");
                    continue;
            }
        }
    }

    void IMenu.RedirectTo(string userInput)
    {
        var game = NewGameMenuItems[userInput];
        game.GetGame()?.StartGame();
    }

    string IMenu.InitialMenu()
    {
        var retString = NewGameMenuItems.Values.Aggregate("============================\n",
            (current, menu) => current + menu + "\n");
        retString += "B) Go back \n";
        retString += "============================";
        return retString;
    }
    
}