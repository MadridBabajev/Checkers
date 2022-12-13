
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CheckersWebApp.Pages.CheckersOptions;

public class DetailsModel : PageModel
{
    private readonly DAL.Db.ApplicationDbContext _context;

    public DetailsModel(DAL.Db.ApplicationDbContext context)
    {
        _context = context;
    }

    public Domain.Db.CheckersOptions CheckersOptions { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var checkersOptions = await _context.CheckersOptions.FirstOrDefaultAsync(m => m.Id == id);
        if (checkersOptions == null)
        {
            return NotFound();
        }

        CheckersOptions = checkersOptions;
        return Page();
    }
}

