using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Db;
using Domain.Db;

namespace CheckersWebApp.Pages.CheckersGames;
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public CheckersGame CheckersGame { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var checkersGame =  await _context.CheckersGames.FirstOrDefaultAsync(m => m.Id == id);
        
        if (checkersGame == null) return NotFound();
        
        CheckersGame = checkersGame;

        var lastState = _context.GameStates.Where(gs => gs.CheckersGameId == id)
            .OrderByDescending(gs => gs.CreatedAt)
            .FirstOrDefault();
        
        if (lastState == null)
        {
            ViewData["WhitesLeft"] = "default";
            ViewData["BlacksLeft"] = "default";
        }
        else
        {
            var deserializedState = System.Text.Json.JsonSerializer
                .Deserialize<CurrentGameState>(lastState.SerializedGameState)!;
            ViewData["WhitesLeft"] = deserializedState.WhitesLeft;
            ViewData["BlacksLeft"] = deserializedState.BlacksLeft;
        }
        
        ViewData["GamePlayer1Id"] = new SelectList(_context.Players, "Id", "PlayerName");
        ViewData["GamePlayer2Id"] = new SelectList(_context.Players, "Id", "PlayerName");
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int id,
        int player1, int player2)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        GameRepository repo = new GameRepository(_context);
        CheckersGame = repo.GetGameById(id.ToString());
        
        CheckersGame.GamePlayer1Id = player1;
        CheckersGame.GamePlayer2Id = player2;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (!CheckersGameExists(CheckersGame.Id))
            {
                return NotFound();
            }
            Console.WriteLine(e);
        }

        return RedirectToPage("./Index");
    }

    public string GetGameOptionsName() =>
        _context.CheckersOptions
            .First(o => o.Id == CheckersGame.CheckersOptionsId).Name;

    private bool CheckersGameExists(int id)
    {
      return _context.CheckersGames.Any(e => e.Id == id);
    }

    public bool CheckIfFinishedState()
    {
        return CheckersGame.GameOverAt != null;
    }
}

