
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Db;

namespace CheckersWebApp.Pages.Players;

public class CreateModel : PageModel
{
    private readonly DAL.Db.ApplicationDbContext _context;

    public CreateModel(DAL.Db.ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Player Player { get; set; }
    

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Players.Add(Player);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

