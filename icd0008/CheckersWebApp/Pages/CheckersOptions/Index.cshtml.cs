
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Db;
using Microsoft.AspNetCore.Mvc;

namespace CheckersWebApp.Pages.CheckersOptions;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Domain.Db.CheckersOptions> CheckersOptions { get;set; } = default!;

    public async Task OnGetAsync()
    {
        CheckersOptions = await _context.CheckersOptions.ToListAsync();
    }
    
    public async Task<IActionResult> OnPostAsync(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var options = await _context.CheckersOptions.FindAsync(int.Parse(id));

        if (options != null)
        {
            _context.CheckersOptions.Remove(options);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

