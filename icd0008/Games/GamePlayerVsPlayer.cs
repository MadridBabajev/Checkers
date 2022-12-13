using System.Globalization;
using System.Text.Json;
using DAL.FileSystem;
using Domain;
using GameOptions;
using GameParts;

namespace Games;

public class GamePlayerVsPlayer: IGame
{
    
    private const string OptionsPath = GlobalConstants.GlobalConstants.OptionsFileLocation;
    private Options? _gamesOptions;
    private List<string?>? _heightSpecifiers;
    private List<string?>? _widthSpecifiers;
    private List<CheckersPiece>? _checkersPieces;
    private bool _whitesTurn;
    private bool GameOver { get; set; }

    private void GamePlayerVsPlayerFactoryNewGameStarted()
    {
        _gamesOptions = GetOptions();
        var savedDataFromUi = GameUI.GameUI.BuildInitialBoard(_gamesOptions);
        _heightSpecifiers = savedDataFromUi.HeightSpecifiers;
        _widthSpecifiers = savedDataFromUi.WidthSpecifiers;
        _checkersPieces = savedDataFromUi.CheckersPieces;
        _whitesTurn = _gamesOptions?.WhitesFirst ?? true;
    }

    private void GamePlayerVsPlayerFactoryNewGameLoaded(Game game)
    {
        _gamesOptions = game.GameOptions;
        _heightSpecifiers = game.HeightSpecifiers;
        _widthSpecifiers = game.WidthSpecifiers;
        _checkersPieces = game.BoardState;
        _whitesTurn = game.WhitesTurn ?? true;
    }
    public void StartGame()
    {
        Console.WriteLine("== Starting new game... ==");
        GamePlayerVsPlayerFactoryNewGameStarted();

        Console.WriteLine("== New Game has started with the following rules: ==");
        Console.Write(_gamesOptions);
        ((IGame)this).InGameMenu();
        ProceedToSaveGame();
    }
    
    public void LoadGame(Game game)
    {
        GamePlayerVsPlayerFactoryNewGameLoaded(game);
        Console.WriteLine("== Loading the game... ==\n");
        RefreshBoard();
        Console.WriteLine("== Game was loaded with the following rules: ==");
        Console.Write(_gamesOptions);
        ((IGame)this).InGameMenu();
        ProceedToSaveGame();
    }
    
    void IGame.InGameMenu()
    {
        Console.WriteLine("\nPress B to go back or R to refresh the board!");
        do
        {
            CheckIfTheGameIsOver();
            if (GameOver) return;
            Console.WriteLine($"=== Currently is {(_whitesTurn ? "White" : "Black")}'s turn ===");
            Console.Write("Choose the piece you want to move [e.g 3-A]: ");
            var userInput = Console.ReadLine()?.ToUpper().Trim();
            if (userInput == "B") return;
            if (userInput == "R") { RefreshBoard(); continue;}
            if (!CheckForAValidMove(userInput)) continue;
            if (!MakeAMove(userInput)) continue;
            _whitesTurn = !_whitesTurn;
        } while (true);
    }
    private bool MakeAMove(string? userInput)
    {
        Console.WriteLine("\nPress B to go back and R to refresh");
        Console.WriteLine($"Currently selected piece: {userInput}");
        while (true)
        {
            Console.Write("Choose where you want to move the piece [e.g 3-A]: ");
            var movesThePieceTo = Console.ReadLine()?.ToUpper().Trim();
            if (movesThePieceTo == "B") return false;
            if (movesThePieceTo == "R"){RefreshBoard(); continue;}
            List<List<string>>? availableMovesForACertainPiece = GetAvailableMoves();
            if (availableMovesForACertainPiece == null) return false;
            List<string?> userMove = new()
            {
                movesThePieceTo?.Split("-")[0],
                movesThePieceTo?.Split("-")[1]
            };
            if (availableMovesForACertainPiece.Any(
                    move => move.SequenceEqual(userMove)))
            {
                UpdateCurrentPieceCoordinates();
                return true;
            }
        }
    }
    private List<List<string>>? GetAvailableMoves()
    {
        /* TODO MAJOR if this method gets finished, Console app will be finished, but the logic is hard
         * TODO This method calculates all the possible move according to the
         * TODO options, piece's color, queen or not etc.
         */
        try
        {
            throw new NotImplementedException();
        }
        catch (NotImplementedException)
        {
            return null;
        }
    }
    private void UpdateCurrentPieceCoordinates()
    {
        // TODO Updates selected piece's coordinates 
        throw new NotImplementedException();
    }
    private bool CheckForAValidMove(string? userInput)
    {
        try
        {
            if (!ValidInputs(userInput?.Split("-")[0],
                    userInput?.Split("-")[1])) return false;
            return true;
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine($"== Invalid input format ({userInput}) ==");
            return false;
        }
    }

    private bool ValidInputs(string? heightSpecifier, string? widthSpecifier)
    {
        if (!(bool)_heightSpecifiers?.Contains(heightSpecifier) 
            || !(bool)_widthSpecifiers?.Contains(widthSpecifier))
        {
            Console.WriteLine("== Specifier out of bound or invalid symbol! ==\n");
            return false;
        }
        List<string?> currentMove = new() { heightSpecifier, widthSpecifier };
        List<List<string?>> boardValidPieces = GetValidPiecesList(_whitesTurn);

        // bool userHasChosenAValidPiece = boardValidPieces.Contains(currentMove);
        bool userHasChosenAValidPiece = boardValidPieces.Any(
            pieceCoordinates => pieceCoordinates.SequenceEqual(currentMove));
        if (!userHasChosenAValidPiece)
        {
            Console.WriteLine($"== No such {(_whitesTurn ? "White" : "Black")} piece found ==");
            return false;
        }
        
        FindChosenPiece();
        return true;
    }
    private List<List<string?>> GetValidPiecesList(bool whitesTurn)
    {
        List<List<string?>> retList = new();
        List<CheckersPiece> checkersPieces;
        if (whitesTurn)
        {
            checkersPieces = _checkersPieces?.FindAll(piece =>
                piece.Color == EPieceColor.White) ?? new List<CheckersPiece>();
        }
        else
        {
            checkersPieces = _checkersPieces?.FindAll(piece =>
                    piece.Color == EPieceColor.Black) ?? new List<CheckersPiece>();
        }

        foreach (CheckersPiece piece in checkersPieces)
        {
            List<string?> pieceCoordinates = ConvertPieceCoordinatesToList(
                piece.YCoordinate, piece.XCoordinate);
            retList.Add(pieceCoordinates);
        }
        return retList;
    }
    private List<string?> ConvertPieceCoordinatesToList(short pieceYCoordinate, short pieceXCoordinate)
    {
        List<string?> retList = new()
        {
            _heightSpecifiers?[pieceYCoordinate],
            _widthSpecifiers?[pieceXCoordinate]
        };
        return retList;
    }
    private void FindChosenPiece()
    {
        // var yCoordinate = (short?)_heightSpecifiers?.IndexOf(heightSpecifier);
        // var xCoordinate = (short?)_widthSpecifiers?.IndexOf(widthSpecifier);
    }

    private void ProceedToSaveGame()
    {
        string? userChoice;
        do
        {
            Console.Write("Save Current Settings?[Y/N]: ");
            userChoice = Console.ReadLine()?.ToUpper().Trim();
        } while (userChoice is not ("N" or "Y"));
        if (userChoice == "N") return;
        Console.WriteLine("Saving the game..");
        SaveGame();
    }
    private void CheckIfTheGameIsOver()
    {
        short whitePiecesCounter = 0;
        short blackPiecesCounter = 0;
        if (_checkersPieces != null)
            foreach (CheckersPiece piece in _checkersPieces)
            {
                if (piece.Color == EPieceColor.White)
                {
                    whitePiecesCounter++;
                }
                else
                {
                    blackPiecesCounter++;
                }
            }
        GameOver = whitePiecesCounter == 0 || blackPiecesCounter == 0;
        if (!GameOver) return;
        Console.WriteLine("Game was finished!");
        Console.WriteLine($"The winner is: {(whitePiecesCounter == 0? "Blacks" : "Whites")}");
    }
    private void RefreshBoard()
    {
        GameUI.GameUI.UpdateBoardState(_checkersPieces, _gamesOptions);
    }

    public Options? GetOptions()
    {
        var jsonDataOptionsTemp = File.ReadAllText(OptionsPath);
        return JsonSerializer.Deserialize<Options>(jsonDataOptionsTemp);
    }
    public void SaveGame()
    {
        string id = EGameType.Pvp + "_" + DateTime.Now.ToString(CultureInfo.InvariantCulture);
        GameRepositoryFileSystem.SaveGame(
            id, _gamesOptions, EGameType.Pvp, _checkersPieces, _heightSpecifiers, _widthSpecifiers, _whitesTurn, DateTime.Now);
    }
    private static void Main() {}
}