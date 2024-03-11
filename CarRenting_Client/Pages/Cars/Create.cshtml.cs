
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using System.Net.Http.Headers;
using BusinessObject.DTO;
using Newtonsoft.Json;
using System.Text;
using BusinessObject.Models.CarModels;
using Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CarRenting_Client.Pages.Cars
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient Client;
        private string ApiUrl = "http://localhost:5209/api/Cars/";
       
      

        public CreateModel()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            Client = new HttpClient(handler);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            Client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        [BindProperty]
        public CarAddDTO Car { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            string json = JsonConvert.SerializeObject(Car);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PostAsync(ApiUrl + "AddCar", content);
            if (response.IsSuccessStatusCode) {
                return RedirectToPage("./Index");
            }
            return BadRequest();
        }
        public SelectList CarBrands { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {         
            string url = "http://localhost:5209/api/CarBrands/CarBrandlist";

            try {
                HttpResponseMessage response = await Client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Throw exception for non-success status codes

                string content = await response.Content.ReadAsStringAsync();
                List<BrandCarDTO> carBrands = JsonConvert.DeserializeObject<List<BrandCarDTO>>(content);

                CarBrands = new SelectList(carBrands, "CarBrandID", "Name"); 

                return Page();
            }
            catch (HttpRequestException ex) {             
                ModelState.AddModelError(string.Empty, "Failed to retrieve car brands.");
                return Page();
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "An error occurred.");
                return Page();
            }
        }


    }
}
