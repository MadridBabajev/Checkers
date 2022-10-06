
using System.Text.Json;
using GameOptions;

namespace MenuSystem;

public class OptionsMenu: IMenu
{
    private const string OptionsPath = GlobalConstants.GlobalConstants.OptionsFileLocation;
    private Options? _currentOptions;

    public void InitialiseMenu()
    {
        var jsonDataOptionsTemp = File.ReadAllText(OptionsPath);
        _currentOptions = JsonSerializer.Deserialize<Options>(jsonDataOptionsTemp);
        Console.WriteLine("\n== Options ==");
        while (true)
        {
            Console.WriteLine(((IMenu)this).InitialMenu());
            // Console.WriteLine("[Currently in the new game menu..]");
            Console.Write("Your choice: ");
            var userInput = Console.ReadLine()?.ToUpper().Trim();
            switch (userInput)
            {
                case "WF":
                case "MT":
                case "QM":
                case "BW":
                case "BH":
                    ((IMenu)this).RedirectTo(userInput);
                    break;
                case "B":
                    var jsonDataOptionsSaved = File.ReadAllText(OptionsPath);
                    var defaultOptions = JsonSerializer.Deserialize<Options>(jsonDataOptionsSaved);
                    if (defaultOptions != null 
                        && !defaultOptions.Equals(_currentOptions))
                    {
                        ProceedToSaveOptions();
                    }
                    return;
                default:
                    Console.Write("Invalid input! Choose one of those: " +
                                  "\n[WF, MT, QM, BW, BH, B]\n");
                    continue;
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

    void IMenu.RedirectTo(string userInput)
    {
        var possibleTrueFalseInputs = new HashSet<string>{"T", "F", "B"};
        switch (userInput)
        {
            case "WF":
            case "MT":
            case "QM":
                HandleTrueFalseParameters(possibleTrueFalseInputs, userInput);
                break;
            case "BW":
            case "BH":
                HandleBoardSize(userInput);
                break;
        }
    }
    private void HandleTrueFalseParameters(IReadOnlySet<string> possibleTrueFalseInputs, string userInput)
    {
        string? userSecondInput;
        WriteTrueFalseHandlerIntro(userInput);
        do
        {
            WriteTrueFalseOptions();
            userSecondInput = Console.ReadLine()?.ToUpper().Trim();

        } while (userSecondInput != null
                 && !possibleTrueFalseInputs.Contains(userSecondInput));

        switch (userSecondInput)
        {
            case "T":
                SetTheRequestedValue(userInput, true);
                break;
            case "F":
                SetTheRequestedValue(userInput, false);
                break;
            case "B":
                return;
        }
    }

    private void SetTheRequestedValue(string userInput, bool userSecondInput)
    {
        switch (userInput)
        {
            case "WF":
                if (_currentOptions != null) _currentOptions.WhitesFirst = userSecondInput;
                Console.WriteLine($"Whites First was set to {
                    (userSecondInput ? "TRUE" : "FALSE")}");
                break;
            case "MT":
                if (_currentOptions != null) _currentOptions.MandatoryTake = userSecondInput;
                Console.WriteLine($"Mandatory Take was set to {
                    (userSecondInput ? "TRUE" : "FALSE")}");
                break;
            case "QM":
                if (_currentOptions != null) _currentOptions.QueensHaveOpMoves = userSecondInput;
                Console.WriteLine($"Queen OP Moves was set to {
                    (userSecondInput ? "TRUE" : "FALSE")}");
                break;
        }
    }

    private void WriteTrueFalseHandlerIntro(string userInput)
    {
        switch (userInput)
        {
            case "WF":
                Console.WriteLine("\nYou decided to edit Whites First option!");
                break;
            case "MT":
                Console.WriteLine("\nYou decided to edit Mandatory Take option!");
                break;
            case "QM":
                Console.WriteLine("\nYou decided to edit Queen moves option!");
                break;
        }
    }
    private void WriteTrueFalseOptions()
    {
        Console.WriteLine("Possible options: \n" +
                          "T -> TRUE\n" +
                          "F -> FALSE\n" +
                          "B -> Go Back");
        Console.Write("Your choice: ");
    }
    private void HandleBoardSize(string userInput)
    {
        Console.WriteLine($"\nYou decided to edit Board {
            (userInput == "BW" ? "Width" : "Height")}!");
        string? userSecondInput;
        do
        {
            WriteBoardSizeOptions(userInput);
            userSecondInput = Console.ReadLine()?.ToUpper().Trim();
            if (userSecondInput == "B") return;
        } while (userSecondInput != null
                 && !int.TryParse(userSecondInput, out _)
                 || (short.Parse(userSecondInput!) < 8
                 && short.Parse(userSecondInput!) >= 101));
        switch (userInput)
        {
            case "BW":
                if (_currentOptions != null) _currentOptions
                    .BoardWidth = short.Parse(userSecondInput!); 
                Console.WriteLine($"Board Width was set to {userSecondInput}");
                break;
            case "BH":
                if (_currentOptions != null) _currentOptions
                    .BoardHeight = short.Parse(userSecondInput!); 
                Console.WriteLine($"Board Height was set to {userSecondInput}");
                break;
        }
    }

    private void WriteBoardSizeOptions(string userInput)
    {
        Console.WriteLine("\nThe current field value must be larger than 8 and less than 101!");
        Console.WriteLine("Press B to go back!");
        Console.Write($"Input the {(userInput == "BW" ? "Width" : "Height")}: ");
    }

    string IMenu.InitialMenu()
    {
        // Actually reads valid json from file
        // var jsonDataOptions = File.ReadAllText(OptionsPath);
        // var optionsObj = JsonSerializer.Deserialize<Options>(jsonDataOptions);
        return "=======================\n" +
               "WF) Whites First\n" +
               "MT) Mandatory Take\n" +
               "QM) Queens OP Moves\n" +
               "BW) Board Width\n" +
               "BH) Board Height\n" +
               "B) Go back \n" +
               "=======================";
    }
}
