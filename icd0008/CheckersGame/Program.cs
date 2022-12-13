// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using GameOptions;
using MenuSystem;

namespace CheckersGame;

internal static class CheckersGame
{
    private static readonly Options DefaultOptions = new(true, true, false, 8, 8);
    // Absolute path on my machine, the relative path doesn't seem to work for me..
    private const string OptionsPath = GlobalConstants.GlobalConstants.OptionsFileLocation;

    public static void Main()
    {
        SetDefaultGameSetting();
        IMenu mainMenu = new MainMenu();
        mainMenu.InitialiseMenu();

    }

    private static void SetDefaultGameSetting()
    {
        var jsonOptionsString = JsonSerializer.Serialize(DefaultOptions);
        File.WriteAllText(OptionsPath, jsonOptionsString);
    }
}