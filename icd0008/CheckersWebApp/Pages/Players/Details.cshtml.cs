
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Db;

namespace CheckersWebApp.Pages.Players;

public class DetailsModel : PageModel
{
    private readonly DAL.Db.ApplicationDbContext _context;

    public DetailsModel(DAL.Db.ApplicationDbContext context)
    {
        _context = context;
    }

  public Player Player { get; set; } = default!;

  public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var player = await _context.Players.FirstOrDefaultAsync(m => m.Id == id);
        if (player == null)
        {
            return NotFound();
        }

        Player = player;
        return Page();
    }

    public List<CheckersGame> GetGamesByDate()
    {
        var games = _context.CheckersGames.Where(g => g.GamePlayer1Id == Player.Id
                                                      || g.GamePlayer2Id == Player.Id).ToList();
        
        games.Sort((g1, g2) => g1.StartedAt.CompareTo(g2.StartedAt));
        return games;
    }
    
    public async Task<IActionResult> OnPostAsync(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var record = await _context.CheckersGames.FindAsync(int.Parse(id));

        if (record != null)
        {
            _context.CheckersGames.Remove(record);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

