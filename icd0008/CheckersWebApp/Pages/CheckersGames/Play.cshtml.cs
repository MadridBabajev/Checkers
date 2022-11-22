
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
    [BindProperty]
    public global::GameBrain.GameBrain GameBrain { get; set; }

    public Play(ApplicationDbContext ctx)
    {
        _gameRepo = new GameRepository(ctx);
    }

    private void PlayFactory(int? id)
    {
        
        CheckersGame = _gameRepo.GetGameById(id?.ToString()!)!;

        Options = _gameRepo.GetOptionsById(CheckersGame.CheckersOptionsId);
        CheckersGame.GamePlayer1 = _gameRepo.GetPlayerById(CheckersGame.GamePlayer1Id);
        CheckersGame.GamePlayer2 = _gameRepo.GetPlayerById(CheckersGame.GamePlayer2Id);
        
        var gameState = CheckersGame.GameStates?.LastOrDefault() ?? null;
        
        GameBrain = new GameBrain.GameBrain(Options, gameState);
        
        // ViewData["game"] = CheckersGame;
        // ViewData["options"] = Options;
        // ViewData["player1"] = CheckersGame.GamePlayer1;
        // ViewData["player2"] = CheckersGame.GamePlayer2;
        ViewData["brain"] = GameBrain;
        
        if (gameState != null) return;
        string serialisedState = System.Text.Json.JsonSerializer
            .Serialize(GameBrain.GetCurrentGameState());
        _gameRepo.AddState(serialisedState, id);
    }

    public Task<IActionResult> OnGet(int? id)
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
        catch (Exception e)
        {
            return new JsonResult(retList);
        }

        retList = GameBrain.GetPossibleMoves(x, y);

        return new JsonResult(retList);
    }

}
