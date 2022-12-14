using Domain.Db;

namespace AiMoveHandler;
using GameBrain;
public static class AiMoveHandler
{
    // private const int Delay = 2;
    private const int MaxDepth = 5;
    private const double QueenEvaluation = 0.75;
    
    // Calculates the best possible move using MiniMax algorithm
    public static Task<Move?> GetAiMove(GameBrain brain)
    {
        if (brain.GameIsOver()) return Task.FromResult<Move?>(null);
        
        var rootPlayer = brain.GetCurrentGameState().CurrentMoveByWhite
            ? EPieceColor.White
            : EPieceColor.Black;
        var copiedBrain = DeepClone(brain);
        Move? bestMove = GetRandomMove(copiedBrain, rootPlayer);
        // Move? bestMove;
        //
        // try
        // {
        //     bestMove = MiniMax(
        //         null!, 0, true, rootPlayer, copiedBrain);
        // }
        // catch (Exception)
        // {
        //     bestMove = GetRandomMove(copiedBrain, rootPlayer);
        // }
        // var timer = new PeriodicTimer(TimeSpan.FromSeconds(Delay));
        
        return Task.FromResult(bestMove);
    }

    private static Move? GetRandomMove(GameBrain game, EPieceColor rootPlayer)
    {
        var allCurrentMoves = GetAllPossibleMoves(GetPiecesOfColor(
            game.GetCurrentGameState().GameBoard, rootPlayer), game);
        if (allCurrentMoves.Count == 0) return null;
        Random rnd = new Random();
        int num = rnd.Next(0, allCurrentMoves.Count);
        return allCurrentMoves[num];
    }

    private static Move MiniMax(
        Move bestMoveToMake, int depth, bool maxPlayer, EPieceColor root, GameBrain game)
    {
        if (depth == MaxDepth || game.GameIsOver()) 
            return bestMoveToMake;
        
        EPieceColor currentPlayerColor = maxPlayer ? root 
            : root == EPieceColor.White ? EPieceColor.Black : EPieceColor.White;
        var miniMaxValue = maxPlayer ? double.MinValue : double.MaxValue;
        Move? bestMove = null;

        foreach (var move in GetAllPossibleMoves(GetPiecesOfColor(
                     game.GetCurrentGameState().GameBoard, currentPlayerColor), game))
        {
            GameBrain gameCloned = DeepClone(game);
            Move currentMove = gameCloned.MakeAiMove(move);
            Move potentialBestMove = MiniMax(
                currentMove, depth + 1, !maxPlayer, root, gameCloned);
            
            double stateEvaluation = EvaluateBoardState(
                gameCloned.GetCurrentGameState(), root);
            double newPossibleMiniMaxValue = maxPlayer
                ? Math.Max(miniMaxValue, stateEvaluation)
                : Math.Min(miniMaxValue, stateEvaluation);

            if (maxPlayer)
            { if (newPossibleMiniMaxValue >= miniMaxValue) bestMove = potentialBestMove; }
            else 
            { if (newPossibleMiniMaxValue <= miniMaxValue) bestMove = potentialBestMove; }
        }

        return bestMove!;
    }

    private static List<Move> GetAllPossibleMoves(List<CheckersPiece> piecesOfColor, GameBrain game)
    {
        List<Move> moves = new();
        foreach (var piece in piecesOfColor)
        {
            List<List<int>> pieceMoves = game.GetPossibleMoves(
                piece.XCoordinate, piece.YCoordinate);
            foreach (var pieceMove in pieceMoves)
            {
                moves.Add(new Move(piece.XCoordinate, piece.YCoordinate,
                    pieceMove[0], pieceMove[1]));
            }
        }
        return moves;
    }

    private static double EvaluateBoardState(CurrentGameState currentState, EPieceColor root)
    {
        if (root == EPieceColor.White) 
            return currentState.WhitesLeft - currentState.BlacksLeft 
                   + (currentState.WhiteQueens * QueenEvaluation - currentState.BlackQueens * QueenEvaluation);
        return currentState.BlacksLeft - currentState.WhitesLeft 
               + (currentState.BlackQueens * QueenEvaluation - currentState.WhiteQueens * QueenEvaluation);
    }

    private static List<CheckersPiece> GetPiecesOfColor(List<CheckersPiece> allPieces, EPieceColor color) 
        => allPieces.FindAll(p => p.Color == color).ToList();
    
    private static GameBrain DeepClone(GameBrain brainOrig)
    {
        var copiedGameState = brainOrig.GetBackEndStateCopy();
        var copy = new GameBrain(copiedGameState, brainOrig.GetCurrentGameOptions());
        return copy;
    }
}
