
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckersWebApp.Pages.CheckersOptions;

public class CreateModel : PageModel
{
    private readonly DAL.Db.ApplicationDbContext _context;
    private readonly List<int> _allowedBoundaries = new()
    {
        8, 10, 16, 32, 64
    }; 

    public CreateModel(DAL.Db.ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        ViewData["PossibleBounds"] = _allowedBoundaries.ConvertAll(num => 
            new SelectListItem {
                Text = num.ToString(),
                Value = num.ToString(),
                Selected = false
            });
        
        return Page();
    }

    [BindProperty]
    public Domain.Db.CheckersOptions CheckersOptions { get; set; }
    

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
          return Page();
      }

      _context.CheckersOptions.Add(CheckersOptions);
      await _context.SaveChangesAsync();

      return RedirectToPage("./Index");
    }
}

