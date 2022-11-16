
using System.Text.Json;
using GameOptions;

namespace MenuSystem;

public class OptionsMenu: IMenu
{
    private const string OptionsPath = GlobalConstants.GlobalConstants.OptionsFileLocation;
    private Options? _currentOptions;
    private readonly string[] _optionsMenuItems = { "Whites First", "Mandatory Take", "Queens Have OP Moves", "Board Width", "Board Height", "Back" };

    public void InitialiseMenu()
    {
        var jsonDataOptionsTemp = File.ReadAllText(OptionsPath);
        _currentOptions = JsonSerializer.Deserialize<Options>(jsonDataOptionsTemp);
        bool userWantsToExist = false;
        while (!userWantsToExist)
        {
            int userChoice = ConsoleHelper.MultipleChoice(true, _optionsMenuItems);
            switch (userChoice)
            {
                case -1:
                case 5 :   
                    userWantsToExist = true;
                    var jsonDataOptionsSaved = File.ReadAllText(OptionsPath);
                    var defaultOptions = JsonSerializer.Deserialize<Options>(jsonDataOptionsSaved);
                    if (defaultOptions != null 
                        && !defaultOptions.Equals(_currentOptions)) ProceedToSaveOptions();
                    break;
                case 0:
                case 1:
                case 2:
                    HandleTrueFalseParameters(userChoice);
                    break;
                case 3:
                case 4:         
                    HandleBoardSize(userChoice);
                    break;
            }
        }
    }
    private void ProceedToSaveOptions()
    {
        Console.WriteLine("\nCurrent Settings: \n"
                          + _currentOptions);
        string? userChoice;
        do
        {
            Console.Write("Save Current Settings?[Y/N]: ");
            userChoice = Console.ReadLine()?.ToUpper().Trim();
        } while (userChoice is not ("N" or "Y"));
        if (userChoice == "N") return;
        var jsonOptionsString = JsonSerializer.Serialize(_currentOptions);
        File.WriteAllText(OptionsPath, jsonOptionsString);
    }
    private void HandleTrueFalseParameters(int userInput)
    {
        string[] trueFalseOptions = { "TRUE", "FALSE", "BACK" };
        bool userWentBack = false;
        while (!userWentBack)
        {
            int userChoice = ConsoleHelper.MultipleChoice(true, trueFalseOptions);
            switch (userChoice)
            {
                case 0:
                    userWentBack = true;
                    SetTheRequestedValue(userInput, true);
                    break;
                case 1:
                    userWentBack = true;
                    SetTheRequestedValue(userInput, false);
                    break;
                case 2:
                    userWentBack = true;
                    break;
            }
        }
    }

    private void SetTheRequestedValue(int userInput, bool userSecondInput)
    {
        switch (userInput)
        {
            case 0:
                if (_currentOptions != null) _currentOptions.WhitesFirst = userSecondInput;
                Console.WriteLine($"Whites First was set to {
                    (userSecondInput ? "TRUE" : "FALSE")}");
                break;
            case 1:
                if (_currentOptions != null) _currentOptions.MandatoryTake = userSecondInput;
                Console.WriteLine($"Mandatory Take was set to {
                    (userSecondInput ? "TRUE" : "FALSE")}");
                break;
            case 2:
                if (_currentOptions != null) _currentOptions.QueensHaveOpMoves = userSecondInput;
                Console.WriteLine($"Queen OP Moves was set to {
                    (userSecondInput ? "TRUE" : "FALSE")}");
                break;
        }
    }
    private void HandleBoardSize(int userInput)
    {
        Console.WriteLine($"\nYou decided to edit Board {
            (userInput == 3 ? "Width" : "Height")}!");
        string? userSecondInput;
        do
        {
            WriteBoardSizeOptions(userInput);
            userSecondInput = Console.ReadLine()?.ToUpper().Trim();
            if (userSecondInput == "B") return;
        } while (userSecondInput != null
                 && !int.TryParse(userSecondInput, out _));

        short userSecondInputShort = GetValidUserSecondInput(short.Parse(userSecondInput!));

        switch (userInput)
        {
            case 3:
                if (_currentOptions != null) _currentOptions
                    .BoardWidth = userSecondInputShort;
                break;
            case 4:
                if (_currentOptions != null) _currentOptions
                    .BoardHeight = userSecondInputShort;
                break;
        }
    }

    private short GetValidUserSecondInput(short userSecondInput)
    {
        if (userSecondInput <= 8) return 8;
        if (userSecondInput >= 26) return 26;
        return userSecondInput;
    }

    private void WriteBoardSizeOptions(int userInput)
    {
        Console.WriteLine("\nThe current field value must be larger than 8 and less than 101!");
        Console.WriteLine("Otherwise default values will be set [8 or 26]");
        Console.WriteLine("Press B to go back!");
        Console.Write($"Input the {(userInput == 3 ? "Width" : "Height")}: ");
    }
}
