
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
        List<List<int>> retList = new();
        List<List<int>> movesWithTakeAPiece = new();
        List<List<int>> initialPossibleMoves = GetInitialPotentialMoves(x, y);
        var foundMoveTakingAPiece = false;

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
                    // Piece was found diagonally, now it checks, whether it it 2 pieces in a row
                    // e.g piece = 1:1; it checks if there is a piece on 2:2 
                    pieceFound = true;
                    List<int> tileInAValidDirection = GetTileInAValidDirection(x, move[0], y, move[1]);
                    var pieceOnFoundTile = FindPieceAt(tileInAValidDirection[0], tileInAValidDirection[1]);
                    // If piece found, define the colors and decide whether the move is possible
                    // (Return a list with a single element if mandatory take is on)
                    if (pieceOnFoundTile == null)
                    {
                        // Opponents tile was on the way
                        if ((_backEndState.CurrentMoveByWhite && piece.Color == EPieceColor.Black)
                            || !_backEndState.CurrentMoveByWhite && piece.Color == EPieceColor.White)
                        {
                            // Check if the move is within the board limits
                            if (NotMoveOutsideOfBorders(tileInAValidDirection))
                            {
                                foundMoveTakingAPiece = true;
                                movesWithTakeAPiece.Add(tileInAValidDirection);
                                retList.Add(tileInAValidDirection);
                            }
                        }
                    }
                }
            }
            // For loop ended, check if any piece was found.
            if (!pieceFound && MoveCanBeMadeForThisPiece(x, y, move)) retList.Add(move);
        }
        // if multiple pieces can be taken, give user opportunity to decide which piece to take
        if (_options.MandatoryTake && foundMoveTakingAPiece)
        {
            List<List<int>> temp = new();
            foreach (var finalMove in movesWithTakeAPiece)
            {
                temp.Add(finalMove);
            }
            return temp;
        }

        return retList;
    }
    
    private bool MoveCanBeMadeForThisPiece(int x, int y, List<int> move)
    {
        CheckersPiece selected = FindPieceAt(x, y)!;
        if (!selected.IsQueen)
        {
            if (selected.Color == EPieceColor.White && move[1] > y) return false;
            if (selected.Color == EPieceColor.Black && move[1] < y) return false;
            return true;
        }
        return true;
    }

    private bool NotMoveOutsideOfBorders(List<int> tileInAValidDirection)
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
        
        if (xDirection > 0) retList.Add(xToward + 1);
        else retList.Add(xToward - 1);
        
        if (yDirection > 0) retList.Add(yToward + 1);
        else retList.Add(yToward - 1);

        return retList;
    }

    private List<List<int>> GetInitialPotentialMoves(int x, int y)
    {
        if (_options.QueensHaveOpMoves && FindPieceAt(x, y)!.IsQueen) 
            return GetInitialPotentialMovesForOpQueen(x, y);
        List<List<int>> retList = new();
        int bottomLimit = _options.BoardHeight;
        int rightLimit = _options.BoardWidth;
        if (x + 1 != rightLimit)
        {
            if (y + 1 != bottomLimit) retList.Add(new List<int>{x+1, y+1});
            
            if (!(y - 1 < 0)) retList.Add(new List<int>{x+1, y-1});
            
        }
        if (!(x - 1 < 0))
        {
            if (y + 1 != bottomLimit) retList.Add(new List<int>{x-1, y+1});
            
            if (!(y - 1 < 0)) retList.Add(new List<int>{x-1, y-1});
        }

        return retList;
    }

    private List<List<int>> GetInitialPotentialMovesForOpQueen(int selectedX, int selectedY)
    {
        List<List<int>> retList = new();

        int bottomLimit = _options.BoardHeight;
        int rightLimit = _options.BoardWidth;
        short counter = 1;
        while (true)
        {
            int newXCoordinate = selectedX + counter;
            int newYCoordinate = selectedY + counter;
            if (FindPieceAt(newXCoordinate, newYCoordinate) != null)
            {
                retList.Add(new List<int>{newXCoordinate, newYCoordinate});
                break;
            }
            if (newXCoordinate == rightLimit || newYCoordinate == bottomLimit) break;
            retList.Add(new List<int>{newXCoordinate, newYCoordinate});
            counter++;
        }

        counter = 1;
        while (true)
        {
            int newXCoordinate = selectedX + counter;
            int newYCoordinate = selectedY - counter;
            if (FindPieceAt(newXCoordinate, newYCoordinate) != null)
            {
                retList.Add(new List<int>{newXCoordinate, newYCoordinate});
                break;
            }
            if (newXCoordinate == rightLimit || newYCoordinate == -1) break;
            retList.Add(new List<int>{newXCoordinate, newYCoordinate});
            counter++;
        }

        counter = 1;
        while (true)
        {
            int newXCoordinate = selectedX - counter;
            int newYCoordinate = selectedY - counter;
            if (FindPieceAt(newXCoordinate, newYCoordinate) != null)
            {
                retList.Add(new List<int>{newXCoordinate, newYCoordinate});
                break;
            }
            if (newXCoordinate == -1 || newYCoordinate == -1) break;
            retList.Add(new List<int>{newXCoordinate, newYCoordinate});
            counter++;
        }

        counter = 1;
        while (true)
        {
            int newXCoordinate = selectedX - counter;
            int newYCoordinate = selectedY + counter;
            if (FindPieceAt(newXCoordinate, newYCoordinate) != null)
            {
                retList.Add(new List<int>{newXCoordinate, newYCoordinate});
                break;
            }
            if (newXCoordinate == -1 || newYCoordinate == bottomLimit) break;
            retList.Add(new List<int>{newXCoordinate, newYCoordinate});
            counter++;
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

        CheckersPiece selected = FindPieceAt(xFrom, yFrom)!;
        selected.XCoordinate = (short) xTo;
        selected.YCoordinate = (short) yTo;
        
        var pieceTaken = false;
        CheckersPiece? pieceToRemove = null;
        
        if (selected.IsQueen && _options.QueensHaveOpMoves)
        {
            pieceToRemove = GetPieceToRemoveForOpQueen(xFrom, yFrom, xTo, yTo);
        }
        else
        {
            if (GetDiagonalDistanceOfTheMove(xFrom, yFrom, xTo, yTo) > 2)
            {
                int xToRemove = (xFrom + xTo) / 2;
                int yToRemove = (yFrom + yTo) / 2;
                pieceToRemove = FindPieceAt(xToRemove, yToRemove)!;
            }
        }

        if (pieceToRemove != null)
        {
            switch (pieceToRemove.Color)
            {
                case EPieceColor.White:
                    _backEndState.WhitesLeft--;
                    break;
                case EPieceColor.Black:
                    _backEndState.BlacksLeft--;
                    break;
            }
            
            _backEndState.GameBoard.Remove(pieceToRemove);
            pieceTaken = true;
        }

        if (!MorePiecesCanBeTaken(selected, pieceTaken)) _backEndState.CurrentMoveByWhite = !_backEndState.CurrentMoveByWhite;
        
        if (PieceReachedTheEnd(selected, yTo)) selected.IsQueen = true;
        FrontEndState =
            WebUIBoardHandler.CreateFrontEndBoard(_backEndState.GameBoard,
                _options.BoardHeight, _options.BoardWidth);
    }

    private CheckersPiece? GetPieceToRemoveForOpQueen(int xFrom, int yFrom, int xTo, int yTo)
    {
        var xDirection = xTo - xFrom;
        var yDirection = yTo - yFrom;
        int xRemoveCoordinate;
        int yRemoveCoordinate;
        
        if (xDirection > 0)
        {
            xRemoveCoordinate = xTo - 1;
            if (yDirection > 0) yRemoveCoordinate = yTo - 1;
            else yRemoveCoordinate = yTo + 1;
        }
        else
        {
            xRemoveCoordinate = xTo + 1;
            if (yDirection > 0) yRemoveCoordinate = yTo - 1;
            else yRemoveCoordinate = yTo + 1;
        }

        var retPiece = FindPieceAt(xRemoveCoordinate, yRemoveCoordinate);
        return retPiece ?? null;
    }

    private bool MorePiecesCanBeTaken(CheckersPiece movedPiece, bool pieceTaken)
    {
        if (!pieceTaken) return false;
        
        var possibleMoves = GetPossibleMoves(movedPiece.XCoordinate, movedPiece.YCoordinate);
        if (movedPiece.IsQueen && _options.QueensHaveOpMoves)
        {
            List<int> rightUpMove = new() { movedPiece.XCoordinate + 1, movedPiece.YCoordinate - 1 };
            List<int> rightDownMove = new() { movedPiece.XCoordinate + 1, movedPiece.YCoordinate + 1 };
            List<int> leftDownMove = new() { movedPiece.XCoordinate - 1, movedPiece.YCoordinate + 1 };
            List<int> leftUpMove = new() { movedPiece.XCoordinate - 1, movedPiece.YCoordinate - 1 };
            foreach (var move in possibleMoves)
            {
                if ( (move[0] == rightUpMove[0] && move[1] == rightUpMove[1])
                    || (move[0] == rightDownMove[0] && move[1] == rightDownMove[1])
                    || (move[0] == leftDownMove[0] && move[1] == leftDownMove[1])
                    || (move[0] == leftUpMove[0] && move[1] == leftUpMove[1])) 
                    return false;
            }
            return true;
        }
        
        foreach (var move in possibleMoves)
        {
            if (GetDiagonalDistanceOfTheMove(movedPiece.XCoordinate, movedPiece.YCoordinate,
                    move[0], move[1]) > 2) return true;
        }
        return false;
    }
    
    private bool PieceReachedTheEnd(CheckersPiece selectedPiece, int yNew)
    {
        switch (selectedPiece.Color)
        {
            case EPieceColor.White:
                if (yNew == 0 && !selectedPiece.IsQueen) return true;
                break;
            case EPieceColor.Black:
                if (yNew == _options.BoardHeight - 1 && !selectedPiece.IsQueen) return true;
                break;
        }
        return false;
    }
    
    private double GetDiagonalDistanceOfTheMove(int xFrom, int yFrom, int xTo, int yTo)
        // c = √((xA − xB)^2 + (yA − yB)^2)
        // c -> distance
        => Math.Sqrt(Math.Pow(xTo-xFrom,2) + Math.Pow(yTo-yFrom,2));
        
}
