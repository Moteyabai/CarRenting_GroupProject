using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;

namespace CarRenting_Client.Pages.CarBrands
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.CarRentingDBContext _context;

        public EditModel(DataAccess.CarRentingDBContext context)
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

            var carbrand =  await _context.CarBrands.FirstOrDefaultAsync(m => m.CarBrandID == id);
            if (carbrand == null)
            {
                return NotFound();
            }
            CarBrand = carbrand;
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

            _context.Attach(CarBrand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarBrandExists(CarBrand.CarBrandID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CarBrandExists(int id)
        {
          return (_context.CarBrands?.Any(e => e.CarBrandID == id)).GetValueOrDefault();
        }
    }
}
