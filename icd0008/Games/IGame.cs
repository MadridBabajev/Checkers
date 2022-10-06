using GameOptions;

namespace Games;

public interface IGame
{
    public void StartGame();
    void InGameMenu();
    public void SaveGame();
    Options? GetOptions();
}