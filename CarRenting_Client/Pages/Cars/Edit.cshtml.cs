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
using System.Net.Http.Headers;
using BusinessObject.DTO;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;

namespace CarRenting_Client.Pages.Cars
{
    public class EditModel : PageModel
    {
        private readonly HttpClient Client;
        private string ApiUrl = "http://localhost:5209/api/Cars/";

        public EditModel()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            Client = new HttpClient(handler);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            Client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        [BindProperty]
        public CarUpdateDTO Car { get; set; } = default!;
        public SelectList CarBrands { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {


            await LoadCarBrands();

            if (id == null) {
                return NotFound();
            }

            HttpResponseMessage response = await Client.GetAsync(ApiUrl + "GetCar/" + id);
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (strData.Equals("\"List is empty!\"")) {
                Car = null;
                return NotFound();
            }
            else {
                Car = System.Text.Json.JsonSerializer.Deserialize<CarUpdateDTO>(strData, options);
                return Page();
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) {
                return Page();
            }

            string json = JsonConvert.SerializeObject(Car);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PutAsync(ApiUrl + "UpdateCar", content);
            if (response.IsSuccessStatusCode) {
                return RedirectToPage("./Index");
            }
            return BadRequest();
        }
        private async Task LoadCarBrands()
        {
            string url = "http://localhost:5209/api/CarBrands/CarBrandlist";

            try {
                HttpResponseMessage response = await Client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Throw exception for non-success status codes

                string content = await response.Content.ReadAsStringAsync();
                List<BrandCarDTO> carBrands = JsonConvert.DeserializeObject<List<BrandCarDTO>>(content);

                CarBrands = new SelectList(carBrands, "CarBrandID", "Name");
            }
            catch (HttpRequestException ex) {
                ModelState.AddModelError(string.Empty, "Failed to retrieve car brands.");
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "An error occurred.");
            }
        }
    }
}
