using DAL;
using DAL.Db;
using Domain.Db;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Exception = System.Exception;

namespace CheckersWebApp.Pages.CheckersGames;

public class Play : PageModel
{
    
    private IGameRepository _gameRepo;
    [BindProperty]
    public CheckersGame CheckersGame { get; set; } = null!;

    [BindProperty]
    public Domain.Db.CheckersOptions Options { get; set; } = default!;

    [BindProperty] public global::GameBrain.GameBrain GameBrain { get; set; } = default!;

    [BindProperty] public string RenderedFrontEndBoard { get; set; } = default!;

    public Play(ApplicationDbContext ctx)
    {
        _gameRepo = new GameRepository(ctx);
    }

    private void PlayFactory(int id, bool playerSurrenderRequest = false)
    {
        // make persistent objects, remember, html is stateless
        // and the objects initially created will be gone, find your way around it
        // Right now there is a lot of queries made after just 1 click
        CheckersGame = _gameRepo.GetGameById(id.ToString())!;

        Options = _gameRepo.GetOptionsById(CheckersGame.CheckersOptionsId);
        CheckersGame.GamePlayer1 = _gameRepo.GetPlayerById(CheckersGame.GamePlayer1Id);
        CheckersGame.GamePlayer2 = _gameRepo.GetPlayerById(CheckersGame.GamePlayer2Id);
        
        var gameState = _gameRepo.GetGameLastState(id)?.SerializedGameState;
        
        GameBrain = new GameBrain.GameBrain(Options, gameState);
        while (CurrentMoveByAi() && !GameBrain.GameIsOver())
        {
            
            Move? aiMove = 
                AiMoveHandler.AiMoveHandler.GetAiMove(GameBrain).Result;
            
            if (aiMove == null)
            {
                FinishGame(id);
                break;
            }

            OnGetMakeAMove(id, aiMove.XFrom,
                aiMove.YFrom,
                aiMove.XTo,
                aiMove.YTo,
                true);
            // Delay does not render the board as it commits changes, it just makes it worse
            // Task.Delay(2 * 1000).Wait();
        }
        if (playerSurrenderRequest || GameBrain.GameIsOver()) FinishGame(id);
        RenderedFrontEndBoard = GameBrain.FrontEndState;
    }

    private bool CurrentMoveByAi()
    {
        if (GameBrain.GetCurrentGameState().CurrentMoveByWhite)
        {
            return CheckersGame.GamePlayer1!.PlayerType switch
            {
                EPlayerType.Human => false,
                EPlayerType.Ai => true,
                _ => false
            };
        }
        return CheckersGame.GamePlayer2!.PlayerType switch
        {
            EPlayerType.Human => false,
            EPlayerType.Ai => true,
            _ => false
        };
    }

    private void FinishGame(int id)
    {
        CheckersGame.GameWonByPlayer = GameBrain.GetCurrentGameState().CurrentMoveByWhite
            ? CheckersGame.GamePlayer2
            : CheckersGame.GamePlayer1;
        
        CheckersGame.GameOverAt = DateTime.Now;
        CheckersGame temp = CheckersGame;
        _gameRepo.DeleteGame(id.ToString());
        _gameRepo.SaveGame(id.ToString(), temp);
    }

    public Task<IActionResult> OnGet(int id)
    {
        try
        {
            PlayFactory(id);
        }
        catch (Exception)
        {
            return Task.FromResult<IActionResult>(NotFound());
        }

        return Task.FromResult<IActionResult>(Page());
    }

    public JsonResult OnGetRestartGame(int id)
    {
        _gameRepo.DeleteAllGameStates(id);
        PlayFactory(id);
        CheckersGame.GameWonByPlayer = null;
        CheckersGame.GameOverAt = null;
        CheckersGame temp = CheckersGame;
        _gameRepo.DeleteGame(id.ToString());
        _gameRepo.SaveGame(id.ToString(), temp);
        PlayFactory(id);
        return new JsonResult("");
    }

    public JsonResult OnGetReverseMove(int id)
    {
        PlayFactory(id);
        if (CheckersGame.GameOverAt != null) return new JsonResult("");
        var lastState = _gameRepo.GetGameLastStateDeserialized(id);
        if (lastState == null) return new JsonResult("");
        _gameRepo.DeleteGameState(lastState);
        PlayFactory(id);
        return new JsonResult("");
    }

    public JsonResult OnGetPlayerSurrender(int id)
    {
        PlayFactory(id, true);
        return new JsonResult("");
    }

    public JsonResult OnGetSelectAPiece(int id, int x, int y)
    {
        List<List<int>> retList = new();
        try
        {
            PlayFactory(id);
        }
        catch (Exception)
        {
            return new JsonResult(retList);
        }
        if (CheckersGame.GameOverAt != null) return new JsonResult("");

        retList = GameBrain.GetPossibleMoves(x, y);

        return new JsonResult(retList);
    }

    public JsonResult OnGetMakeAMove(int id, int xFrom, int yFrom, int xTo, int yTo, bool callFromFactory = false)
    {
        if (!callFromFactory) {
            try
            {
                PlayFactory(id);
            }
            catch (Exception) { // ignored
            }
        }
        
        var lastGameState = _gameRepo.GetGameLastState(id);
        string serializedState;

        if (lastGameState == null)
        {
            serializedState = System.Text.Json.JsonSerializer
                .Serialize(GameBrain.GetCurrentGameState());
            _gameRepo.AddState(serializedState, id);
        }

        GameBrain.MakeAMove(xFrom, yFrom, xTo, yTo);
        
        serializedState = System.Text.Json.JsonSerializer
            .Serialize(GameBrain.GetCurrentGameState());
        
        _gameRepo.AddState(serializedState, id);

        RenderedFrontEndBoard = GameBrain.FrontEndState;
        
        return new JsonResult("");
    }

}
