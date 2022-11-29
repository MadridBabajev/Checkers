
using DAL;
using DAL.Db;
using Domain.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CheckersWebApp.Pages.CheckersGames;

public class Play : PageModel
{
    
    private IGameRepository _gameRepo;
    [BindProperty]
    public CheckersGame CheckersGame { get; set; }
    [BindProperty]
    public Domain.Db.CheckersOptions Options { get; set; }

    [BindProperty] public global::GameBrain.GameBrain GameBrain { get; set; } = default!;

    [BindProperty] public string RenderedFrontEndBoard { get; set; }

    public Play(ApplicationDbContext ctx)
    {
        _gameRepo = new GameRepository(ctx);
    }

    private void PlayFactory(int id)
    {
        
        CheckersGame = _gameRepo.GetGameById(id.ToString())!;

        Options = _gameRepo.GetOptionsById(CheckersGame.CheckersOptionsId);
        CheckersGame.GamePlayer1 = _gameRepo.GetPlayerById(CheckersGame.GamePlayer1Id);
        CheckersGame.GamePlayer2 = _gameRepo.GetPlayerById(CheckersGame.GamePlayer2Id);
        
        var gameState = _gameRepo.GetGameLastState(id)?.SerializedGameState;
        
        GameBrain = new GameBrain.GameBrain(Options, gameState);
        RenderedFrontEndBoard = GameBrain.FrontEndState;
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
        // TODO It successfully deletes the last state, but does not render the board
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
        Console.WriteLine("\n \n===== Player surrender =====\n \n");
        PlayFactory(id);
        CheckersGame.GameWonByPlayer = GameBrain.GetCurrentGameState().CurrentMoveByWhite
            ? CheckersGame.GamePlayer2
            : CheckersGame.GamePlayer1;
        
        CheckersGame.GameOverAt = DateTime.Now;
        CheckersGame temp = CheckersGame;
        _gameRepo.DeleteGame(id.ToString());
        _gameRepo.SaveGame(id.ToString(), temp);
        PlayFactory(id);
        return new JsonResult("");
    }

    public JsonResult OnGetSelectAPiece(int id, int x, int y)
    {
        // TODO make persistent objects, remember, html is stateless
        // TODO and the objects initially created will be gone, find your way around it
        // TODO Right now there is a lot of queries made after just 1 click
        
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

    public JsonResult OnGetMakeAMove(int id, int xFrom, int yFrom, int xTo, int yTo)
    {
        try
        {
            PlayFactory(id);
        }
        catch (Exception)
        {
            Console.WriteLine("Failed");
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
