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
using BusinessObject.DTO;
using System.Text.Json;
using BusinessObject.Models.CarModels;

namespace CarRenting_Client.Pages.Cars
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient Client;
        private string ApiUrl = "http://localhost:5209/api/Cars/";

        public IndexModel()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            Client = new HttpClient(handler);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            Client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public IList<CarViewModels> Car { get;set; } = default!;
        [BindProperty]
        public string Search { get; set; }

        public async Task OnGetAsync()
        {
            HttpResponseMessage response = await Client.GetAsync(ApiUrl + "CarList");

            if (response.IsSuccessStatusCode) {
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                Car = System.Text.Json.JsonSerializer.Deserialize<List<CarViewModels>>(strData, options);

                RedirectToPage("./Index");
            }
            RedirectToPage("./Index");
        }

        public async Task OnPostAsync()
        {
            /*string json = JsonConvert.SerializeObject(Search);*/
            ;
            HttpResponseMessage response = await Client.GetAsync(ApiUrl + "Search/" + Search);

            if (response.IsSuccessStatusCode) {
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                Car = System.Text.Json.JsonSerializer.Deserialize<List<CarViewModels>>(strData, options);

                RedirectToPage("./Index");
            }
            RedirectToPage("./Index");

        }
    }
}
