using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD:CarRenting_Client/Pages/Users/Index.cshtml.cs
using Newtonsoft.Json;
using System.Text;
using BusinessObject.Models.UserModels;
=======
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using System.Net.Http.Headers;
using BusinessObject.DTO;
using System.Text.Json;
using BusinessObject.Models.CarModels;
>>>>>>> 9704aa6c0425cdf5858cb62affc07109fa356863:CarRenting_Client/Pages/Cars/Index.cshtml.cs

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

<<<<<<< HEAD:CarRenting_Client/Pages/Users/Index.cshtml.cs
        public IList<UserViewModel> User { get;set; } = default!;
=======
        public IList<CarViewModels> Car { get;set; } = default!;
>>>>>>> 9704aa6c0425cdf5858cb62affc07109fa356863:CarRenting_Client/Pages/Cars/Index.cshtml.cs
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
<<<<<<< HEAD:CarRenting_Client/Pages/Users/Index.cshtml.cs
                User = System.Text.Json.JsonSerializer.Deserialize<List<UserViewModel>>(strData, options);
=======
                Car = System.Text.Json.JsonSerializer.Deserialize<List<CarViewModels>>(strData, options);
>>>>>>> 9704aa6c0425cdf5858cb62affc07109fa356863:CarRenting_Client/Pages/Cars/Index.cshtml.cs

                RedirectToPage("./Index");
            }
            RedirectToPage("./Index");
        }

        public async Task OnPostAsync()
        {
<<<<<<< HEAD:CarRenting_Client/Pages/Users/Index.cshtml.cs
            var token = HttpContext.Session.GetString("Token");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            /*string json = JsonConvert.SerializeObject(Search);*/;
            if (Search != null)
            {
                HttpResponseMessage response = await Client.GetAsync(ApiUrl + "Search/" + Search);

                if (response.IsSuccessStatusCode)
                {
                    string strData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    User = System.Text.Json.JsonSerializer.Deserialize<List<UserViewModel>>(strData, options);
                    RedirectToPage("./Index");
                }
            }
            else
            {
                HttpResponseMessage response = await Client.GetAsync(ApiUrl + "UserList");
                if (response.IsSuccessStatusCode)
                {
                    string strData = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    User = System.Text.Json.JsonSerializer.Deserialize<List<UserViewModel>>(strData, options);
                    RedirectToPage("./Index");
                }
=======
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
>>>>>>> 9704aa6c0425cdf5858cb62affc07109fa356863:CarRenting_Client/Pages/Cars/Index.cshtml.cs
            }
            RedirectToPage("./Index");
        }
    }
}
