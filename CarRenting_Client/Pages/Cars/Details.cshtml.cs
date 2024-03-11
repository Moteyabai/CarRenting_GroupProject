using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using System.Net.Http.Headers;
using BusinessObject.Models.CarModels;
using BusinessObject.DTO;
using System.Text.Json;

namespace CarRenting_Client.Pages.Cars
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient Client;
        private string ApiUrl = "http://localhost:5209/api/Cars/";

        public DetailsModel()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            Client = new HttpClient(handler);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            Client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public CarViewModels Car { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
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
                Car = System.Text.Json.JsonSerializer.Deserialize<CarViewModels>(strData, options);
                return Page();
            }
        }
    }
}
