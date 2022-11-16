
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Db;

namespace CheckersWebApp.Pages.Players;

public class EditModel : PageModel
{
    private readonly DAL.Db.ApplicationDbContext _context;

    public EditModel(DAL.Db.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Player Player { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var player =  await _context.Players.FirstOrDefaultAsync(m => m.Id == id);
        if (player == null)
        {
            return NotFound();
        }
        Player = player;
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

        _context.Attach(Player).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PlayerExists(Player.Id))
            {
                return NotFound();
            }
            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool PlayerExists(int id)
    {
      return _context.Players.Any(e => e.Id == id);
    }
}
