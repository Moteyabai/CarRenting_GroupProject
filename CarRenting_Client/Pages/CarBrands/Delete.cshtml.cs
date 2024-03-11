using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;

namespace CarRenting_Client.Pages.CarBrands
{
    public class DeleteModel : PageModel
    {
        private readonly DataAccess.CarRentingDBContext _context;

        public DeleteModel(DataAccess.CarRentingDBContext context)
        {
            _context = context;
        }

        [BindProperty]
      public CarBrand CarBrand { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CarBrands == null)
            {
                return NotFound();
            }

            var carbrand = await _context.CarBrands.FirstOrDefaultAsync(m => m.CarBrandID == id);

            if (carbrand == null)
            {
                return NotFound();
            }
            else 
            {
                CarBrand = carbrand;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.CarBrands == null)
            {
                return NotFound();
            }
            var carbrand = await _context.CarBrands.FindAsync(id);

            if (carbrand != null)
            {
                CarBrand = carbrand;
                _context.CarBrands.Remove(CarBrand);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
