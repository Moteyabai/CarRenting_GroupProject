using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;

namespace CarRenting_Client.Pages.CarBrands
{
    public class CreateModel : PageModel
    {
        private readonly DataAccess.CarRentingDBContext _context;

        public CreateModel(DataAccess.CarRentingDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CarBrand CarBrand { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.CarBrands == null || CarBrand == null)
            {
                return Page();
            }

            _context.CarBrands.Add(CarBrand);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
