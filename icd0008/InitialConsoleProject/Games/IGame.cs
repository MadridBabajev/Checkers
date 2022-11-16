using Domain;
using GameOptions;

namespace Games;

public interface IGame
{
    public void StartGame();
    void InGameMenu();
    public void SaveGame();
    public void LoadGame(Game game);
    Options? GetOptions();
}