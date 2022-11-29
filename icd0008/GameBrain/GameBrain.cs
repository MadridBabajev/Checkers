
using Domain.Db;
using WebUIHandler;

namespace GameBrain;

public class GameBrain
{
    private CurrentGameState _backEndState;
    public string FrontEndState = "";
    private readonly CheckersOptions _options;
    // private CheckersPiece? _currentlySelectedPiece = null;
    public GameBrain(CheckersOptions options, string? lastState)
    {
        _options = options;
        if (lastState == null)
        {
            _backEndState = new CurrentGameState();
            InitializeNewGame();
        }
        else
        {
            _backEndState = System.Text.Json.JsonSerializer.Deserialize<CurrentGameState>(lastState)!;
            FrontEndState = WebUIBoardHandler.CreateFrontEndBoard(_backEndState.GameBoard,
                _options.BoardHeight, _options.BoardWidth);
        }
    }

    private void InitializeNewGame()
    {
        _backEndState.GameBoard = new List<CheckersPiece>();
        
        // Set initial checkers coordinates and color

        var whiteRange = _options.BoardHeight - 3;
        var currentlyWhiteTile = false;
        for (short i = 0; i < _options.BoardHeight; i++)
        {

            for (short j = 0; j < _options.BoardWidth; j++)
            {
                
                currentlyWhiteTile = !currentlyWhiteTile;
                if (currentlyWhiteTile)
                {
                    if (j == _options.BoardWidth - 1) currentlyWhiteTile = !currentlyWhiteTile;
                    continue;
                }

                // Adding pieces
                if (i <= 2)
                {
                    _backEndState.GameBoard.Add(new CheckersPiece(j, i, EPieceColor.Black));
                    _backEndState.BlacksLeft++;
                }
                else if (i >= whiteRange)
                {
                    _backEndState.GameBoard.Add(new CheckersPiece(j, i, EPieceColor.White));
                    _backEndState.WhitesLeft++;
                }

                if (j == _options.BoardWidth - 1)
                {
                    currentlyWhiteTile = !currentlyWhiteTile;
                }
            }
        }

        _backEndState.CurrentMoveByWhite = _options.WhitesFirst;
        
        FrontEndState = WebUIBoardHandler.CreateFrontEndBoard(_backEndState.GameBoard,
            _options.BoardHeight, _options.BoardWidth);
        
    }

    public CurrentGameState GetCurrentGameState()
    {
        return _backEndState;
    }

    public List<List<int>> GetPossibleMoves(int x, int y)
    {
        // Console.WriteLine("\n \n ====== Got to brains Congratulations! ====== \n \n");
        
        
        List<List<int>> retList = new();
        List<List<int>> initialPossibleMoves = GetInitialPotentialMoves(x, y);

        // Check if the player picked a piece according to their turn and color.
        if (_backEndState.CurrentMoveByWhite && FindPieceAt(x, y)!.Color == EPieceColor.Black) return retList;
        if (!_backEndState.CurrentMoveByWhite && FindPieceAt(x, y)!.Color == EPieceColor.White) return retList;

        foreach (var move in initialPossibleMoves)
        {
            var pieceFound = false;
            foreach (var piece in _backEndState.GameBoard)
            {
                if (piece.XCoordinate == x && piece.YCoordinate == y) continue;
                if (piece.XCoordinate == move[0] && piece.YCoordinate == move[1])
                {
                    pieceFound = true;
                    List<int> tileInAValidDirection = GetTileInAValidDirection(x, move[0], y, move[1]);
                    var pieceOnFoundTile = FindPieceAt(tileInAValidDirection[0], tileInAValidDirection[1]);
                    
                    // If piece found, define the colors and decide whether the move is possible
                    // (Return a list with a single element)
                    if (pieceOnFoundTile == null)
                    {
                        // Opponents tile was on the way
                        if ((_backEndState.CurrentMoveByWhite && piece.Color == EPieceColor.Black)
                            || !_backEndState.CurrentMoveByWhite && piece.Color == EPieceColor.White)
                        {
                            // Check if the move is within the board limits
                            if (!MoveOutsideOfBorders(tileInAValidDirection))
                            {
                                if (_options.MandatoryTake)
                                {
                                    List<List<int>> newRetList = new() { tileInAValidDirection };
                                    return newRetList;
                                }
                                retList.Add(tileInAValidDirection);
                            }
                        }
                    }
                    break;
                }
            }
            // For loop ended, check if any piece was found.
            if (!pieceFound && MoveCanBeMadeForThisPiece(x, y, move)) retList.Add(move);
        }

        return retList;
    }

    private bool MoveCanBeMadeForThisPiece(int x, int y, List<int> move)
    {
        // TODO MAJOR Also check the piece color whether it can go in that direction or not
        CheckersPiece selected = FindPieceAt(x, y)!;
        if (!selected.IsQueen)
        {
            if (selected.Color == EPieceColor.White && move[1] > y) return false;
            if (selected.Color == EPieceColor.Black && move[1] < y) return false;
            return true;
        }
        Console.WriteLine("Not implemented for OP Queens");
        return true;
    }

    private bool MoveOutsideOfBorders(List<int> tileInAValidDirection)
    {
        return tileInAValidDirection[0] != _options.BoardWidth 
               && tileInAValidDirection[0] != -1 
               && tileInAValidDirection[1] != _options.BoardHeight 
               && tileInAValidDirection[1] != -1;
    }

    private List<int> GetTileInAValidDirection(int xSelected, int xToward, int ySelected, int yToward)
    {
        List<int> retList = new();
        var xDirection = xToward - xSelected;
        var yDirection = yToward - ySelected;
        
        if (xDirection > 0) retList.Add(xSelected + 2);
        else retList.Add(xSelected - 2);
        
        if (yDirection > 0) retList.Add(ySelected + 2);
        else retList.Add(ySelected - 2);

        return retList;
    }

    private List<List<int>> GetInitialPotentialMoves(int x, int y)
    {
        // TODO you can elaborate the queens later, stick with simple queens for now.
        // To receive OP queen move you probably need to write an algorithm
        // that finds all diagonal moves up until the border. Or handle it separately...

        List<List<int>> retList = new();
        int bottomLimit = _options.BoardHeight;
        int rightLimit = _options.BoardWidth;
        if (x + 1 != rightLimit)
        {
            if (y + 1 != bottomLimit)
            {
                retList.Add(new List<int>{x+1, y+1});
            }
            if (!(y - 1 < 0))
            {
                retList.Add(new List<int>{x+1, y-1});
            }
        }
        if (!(x - 1 < 0))
        {
            if (y + 1 != bottomLimit)
            {
                retList.Add(new List<int>{x-1, y+1});
            }
            if (!(y - 1 < 0))
            {
                retList.Add(new List<int>{x-1, y-1});
            }
        }

        return retList;
    }

    private CheckersPiece? FindPieceAt(int x, int y)
    {
        return _backEndState.GameBoard
            .FirstOrDefault(piece => piece.XCoordinate == x && piece.YCoordinate == y);
    }

    public void MakeAMove(int xFrom, int yFrom, int xTo, int yTo)
    {
        // Console.WriteLine("\n \n ====== Make a move inside a game brain ====== \n \n");
        // Console.WriteLine($"===== from x-{xFrom} y-{yFrom} to x-{xTo} y-{yTo} =====");
        
        foreach (var piece in _backEndState.GameBoard)
        {
            Console.WriteLine(piece);
        }
        
        CheckersPiece selected = FindPieceAt(xFrom, yFrom)!;
        selected.XCoordinate = (short) xTo;
        selected.YCoordinate = (short) yTo;

        Console.WriteLine("\n \n ====== After ====== \n \n");
        
        foreach (var piece in _backEndState.GameBoard)
        {
            Console.WriteLine(piece);
        }

        // TODO decide whether the turn is over and remove the necessary piece.

        var pieceWasTaken = false;
        int diagonalDistance = GetDiagonalDistanceOfTheMove(xFrom, yFrom, xTo, yTo);
        if (diagonalDistance == 2)
        {
            int xToRemove = (xFrom + xTo) / 2;
            int yToRemove = (yFrom + yTo) / 2;
            _backEndState.GameBoard.Remove(FindPieceAt(xToRemove, yToRemove)!);
            pieceWasTaken = true;
        }

        if (!pieceWasTaken) _backEndState.CurrentMoveByWhite = !_backEndState.CurrentMoveByWhite;

            // TODO MAJOR render the board after the move
        FrontEndState =
            WebUIBoardHandler.CreateFrontEndBoard(_backEndState.GameBoard,
                _options.BoardHeight, _options.BoardWidth);
    }

    private int GetDiagonalDistanceOfTheMove(int xFrom, int yFrom, int xTo, int yTo)
        // c = √((xA − xB)^2 + (yA − yB)^2)
        // c -> distance
        => Convert.ToInt32(Math.Sqrt(Math.Pow(xTo-xFrom,2) + Math.Pow(yTo-yFrom,2)));
        
}
