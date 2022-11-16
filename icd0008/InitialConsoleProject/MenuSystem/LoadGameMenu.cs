using DAL.FileSystem;
using Domain;
using Games;

namespace MenuSystem;

public class LoadGameMenu: IMenu
{
    private List<string> _loadGameMenuItems = new();
    private Dictionary<int, Game> _savedGames = new();
    public void InitialiseMenu()
    {
        Console.WriteLine("\n== Saved Games ==");
        GetGamesFromRepository();
        bool userWantsToExist = false;
        int lastItemValue = _loadGameMenuItems.Count - 1;
        
        while (!userWantsToExist)
        {
            int userChoice = ConsoleHelper.MultipleChoice(true, _loadGameMenuItems.ToArray());
            if (userChoice == lastItemValue) return;
            switch (userChoice)
            {
                case -1:
                    Console.WriteLine("\nExiting The game..");
                    userWantsToExist = true;
                    break;
                default:
                    LoadSelectedGame(_savedGames[userChoice]);
                    break;
            }
        }
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
    private void GetGamesFromRepository()
    {
        short i = 0;
        _savedGames = new();
        _loadGameMenuItems = new();
        foreach (var game in GameRepositoryFileSystem.GetAllGamesList())
        {
             _savedGames.Add(i++, game);
             _loadGameMenuItems.Add(game.ToString());
        }
        _loadGameMenuItems.Add("Back");
    }

}