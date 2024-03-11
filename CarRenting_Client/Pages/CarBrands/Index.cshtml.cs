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
    public class IndexModel : PageModel
    {
        private readonly DataAccess.CarRentingDBContext _context;

        public IndexModel(DataAccess.CarRentingDBContext context)
        {
            _context = context;
        }

        public IList<CarBrand> CarBrand { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.CarBrands != null)
            {
                CarBrand = await _context.CarBrands.ToListAsync();
            }
        }
    }
}
