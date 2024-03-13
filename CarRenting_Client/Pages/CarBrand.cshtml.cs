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
using BusinessObject.DTO;
using BusinessObject.Models.CarModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CarRenting_Client.Pages
{
    public class CarBrandModel : PageModel
    {
        private readonly string apiSearch = "http://localhost:5209/api/CarBrands/Search/";
        private readonly string apiList = "http://localhost:5209/api/CarBrands/CarBrandlist";
        private readonly string apiID = "http://localhost:5209/api/CarBrands/GetCar";
        private readonly string apiAdd = "http://localhost:5209/api/CarBrands/AddCarBrand";
        private readonly string apiUpdate = "http://localhost:5209/api/CarBrands/UpdateCarBrand";

        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CarBrandID { get; set; }

        [BindProperty]
        public List<BrandCarDTO> CarBrands { get; set; }

        [BindProperty]
        public CarBrandAddDTO CarBrandAdd { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string token = HttpContext.Session.GetString("Token");
            using (var httpClient = new HttpClient())
            {
                // Append the search parameter to the API URL if a name is provided
                string url = string.IsNullOrEmpty(Name) ? apiList : $"{apiSearch}{Name}";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        CarBrands = JsonConvert.DeserializeObject<List<BrandCarDTO>>(apiResponse);
                    }
                    else
                    {
                        CarBrands = null;
                    }

                }
                return Page();
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await httpClient.PostAsJsonAsync(apiAdd, CarBrandAdd);
                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful creation
                        return RedirectToPage();
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        return BadRequest($"Failed to add new brand: {errorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
