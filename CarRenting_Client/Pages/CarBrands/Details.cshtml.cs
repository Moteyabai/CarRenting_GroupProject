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
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.CarRentingDBContext _context;

        public DetailsModel(DataAccess.CarRentingDBContext context)
        {
            _context = context;
        }

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
    }
}
