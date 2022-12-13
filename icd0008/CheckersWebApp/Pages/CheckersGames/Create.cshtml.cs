using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Db;
using Domain.Db;

namespace CheckersWebApp.Pages.CheckersGames;
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        ViewData["CheckersOptionsId"] = new SelectList(_context.CheckersOptions, "Id", "Name");
        ViewData["GamePlayer1Id"] = new SelectList(_context.Players, "Id", "PlayerName");
        ViewData["GamePlayer2Id"] = new SelectList(_context.Players, "Id", "PlayerName");
        return Page();
    }

    [BindProperty]
    public CheckersGame CheckersGame { get; set; } = default!;
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        CheckersGame.StartedAt = DateTime.Today;
        _context.CheckersGames.Add(CheckersGame);
        await _context.SaveChangesAsync();

        return Redirect($"/CheckersGames/Play?id={CheckersGame.Id}");
    }
}