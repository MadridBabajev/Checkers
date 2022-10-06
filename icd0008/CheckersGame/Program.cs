// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using GameOptions;
using MenuSystem;

namespace CheckersGame;

internal static class CheckersGame
{
    private static readonly Options DefaultOptions = new();
    // Absolute path on my machine, the relative path doesn't seem to work for me..
    private const string OptionsPath = @"C:\Users\Madrid Babajev\AllProjects\RiderProjects\icd0008-2022f\icd0008\GameOptions\CurrentOptions.json";

    public static void Main()
    {
        SetDefaultGameSetting();
        IMenu mainMenu = new MainMenu();
        mainMenu.InitialiseMenu();
        // foreach (var fileName in Directory.GetFileSystemEntries("C:\\Users\\Madrid Babajev\\AllProjects\\RiderProjects\\icd0008-2022f\\icd0008\\DAL.FileSystem\\SavedGames", "*.json"))
        // {
        //     Console.WriteLine(Path.GetFileNameWithoutExtension(fileName));
        // }
    }

    private static void SetDefaultGameSetting()
    {
        DefaultOptions.WhitesFirst = true;
        DefaultOptions.MandatoryTake = true;
        DefaultOptions.QueensHaveOpMoves = false;
        DefaultOptions.BoardWidth = 8;
        DefaultOptions.BoardHeight = 8;

        var jsonOptionsString = JsonSerializer.Serialize(DefaultOptions);
        File.WriteAllText(OptionsPath, jsonOptionsString);
    }
}