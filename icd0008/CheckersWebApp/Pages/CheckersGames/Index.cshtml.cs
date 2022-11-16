
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Db;
using Domain.Db;
using Microsoft.AspNetCore.Mvc;

namespace CheckersWebApp.Pages.CheckersGames;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<CheckersGame> CheckersGames { get;set; } = default!;

    public async Task OnGetAsync()
    {
        // Probably exception is coming from here
        
        CheckersGames = await _context.CheckersGames
            .Include(c => c.CheckersOptions)
            .Include(c => c.GamePlayer1)
            .Include(c => c.GamePlayer2)
            .ToListAsync();
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
