using GameOptions;

namespace MenuSystem;

public class LoadGameMenu: IMenu
{


    public void InitialiseMenu()
    {
        Console.WriteLine("\n Not Implemented yet");
        // Console.WriteLine("\n== Select game mode ==");
        // while (true)
        // {
        //     Console.WriteLine(((IMenu)this).InitialMenu());
        //     // Console.WriteLine("[Currently in the new game menu..]");
        //     Console.Write("Your choice: ");
        //     var userInput = Console.ReadLine()?.ToUpper().Trim();
        //     switch (userInput)
        //     {
        //         case "PVP":
        //         case "PVE":
        //         case "EVE":
        //         case "PVP_Online":
        //             ((IMenu)this).RedirectTo(userInput);
        //             break;
        //         case "B":
        //             return;
        //         default:
        //             Console.Write("Invalid input! Choose one of those: " +
        //                           "\n[PVP, PVE, EVE, PVP_Online, B]\n");
        //             continue;
        //     }
        // }
    }
    void IMenu.RedirectTo(string userInput)
    {
        // GameItem game;
        // switch (userInput)
        // {
        //     case "PVP":
        //         game = NewGameMenuItems["PVP"];
        //         game.GetGame()?.StartGame();
        //         Console.WriteLine("You actually chose PVP");
        //         break;
        //     case "PVE":
        //         game = NewGameMenuItems["PVE"];
        //         game.GetGame()?.StartGame();
        //         break;
        //     case "EVE":
        //         game = NewGameMenuItems["EVE"];
        //         game.GetGame()?.StartGame();
        //         break;
        //     case "PVP_Online":
        //         game = NewGameMenuItems["PVP_Online"];
        //         game.GetGame()?.StartGame();
        //         break;
        // }
    }

    string IMenu.InitialMenu()
    {
        // var retString = NewGameMenuItems.Values.Aggregate("============================\n",
        //     (current, menu) => current + menu + "\n");
        // retString += "B) Go back \n";
        // retString += "============================";
        // return retString;
        return "";
    }

}