
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
        if (checkersGame == null)
        {
            return NotFound();
        }
        CheckersGame = checkersGame;
       // ViewData["CheckersOptionsId"] = new SelectList(_context.CheckersOptions, "Id", "Name");
       
       ViewData["GamePlayer1Id"] = new SelectList(_context.Players, "Id", "PlayerName");
       ViewData["GamePlayer2Id"] = new SelectList(_context.Players, "Id", "PlayerName");
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(CheckersGame).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CheckersGameExists(CheckersGame.Id))
            {
                return NotFound();
            }

            throw;
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
