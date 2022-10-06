using DAL.FileSystem;
using Domain;
using Games;

namespace MenuSystem;

public class LoadGameMenu: IMenu
{
    private HashSet<string> _validNums = new();
    private Dictionary<string, Game> _savedGames = new();
    public void InitialiseMenu()
    {
        Console.WriteLine("\n== Saved Games ==");
        Console.WriteLine(((IMenu)this).InitialMenu());
        while (true)
        {
            Console.WriteLine("Pick a game You would like to load![e.g 1]");
            Console.WriteLine("Press B to go back");
            Console.Write("Your choice: "); 
            var userInput = Console.ReadLine()?.ToUpper().Trim();
            if (userInput == "B") break;
            if (!_validNums.Contains(userInput!)) {Console.WriteLine("No such game number found"); continue;}

            ((IMenu)this).RedirectTo(userInput);
        }
    }
    void IMenu.RedirectTo(string? userInput)
    {
        foreach (KeyValuePair<string, Game> entry in _savedGames)
        {
            if (userInput == entry.Key)
            {
                LoadSelectedGame(entry.Value);
            }
        }
        Console.WriteLine("Failed to find the game!");
    }

    private void LoadSelectedGame(Game game)
    {
        switch (game.GameType)
        {
            case EGameType.Pvp:
                new GamePlayerVsPlayer().LoadGame(game);
                break;
            case EGameType.Pve:
                Console.WriteLine("Loading not implemented yet!");
                break;
            case EGameType.Eve:
                Console.WriteLine("Loading not implemented yet!");
                break;
            case EGameType.PvpOnline:
                Console.WriteLine("Loading not implemented yet!");
                break;
        }
    }

    string IMenu.InitialMenu()
    {
        Console.WriteLine("===============================");
        string printString = "";
        short i = 1;
        
        foreach (var game in GameRepositoryFileSystem.GetAllGamesList())
        {
            _validNums.Add(i.ToString());
            _savedGames.Add(i.ToString(), game);
            printString += "===============================\n";
            printString += $"Game number -> {i}\n{game}";
            printString += "===============================\n";
            i++;
        }
        printString += "===============================";
        return printString;
    }

}