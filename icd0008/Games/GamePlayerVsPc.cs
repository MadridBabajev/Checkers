using Domain;
using GameOptions;

namespace Games;

public class GamePlayerVsPc: IGame
{
    public void StartGame()
    {
        throw new NotImplementedException();
    }
    
    public void LoadGame(Game game)
    {
        throw new NotImplementedException();
    }

    void IGame.InGameMenu()
    {
        throw new NotImplementedException();
    }

    public void SaveGame()
    {
        throw new NotImplementedException();
    }

    public Options GetOptions()
    {
        throw new NotImplementedException();
    }
}