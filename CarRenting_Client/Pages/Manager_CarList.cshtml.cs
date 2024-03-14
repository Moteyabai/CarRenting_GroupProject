using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using BusinessObject.Models.CarModels;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CarRenting_Client.Pages
{
    public class Manager_CarListModel : PageModel
    {
        private readonly HttpClient Client;
        private string ApiUrl = "http://localhost:5209/api/Cars/";
        public Manager_CarListModel() 
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            Client = new HttpClient(handler);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            Client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public List<CarViewModels> Car { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("RoleID") == null)
            {
                return RedirectToPage("./Login");
            }
            else
            {
                var token = HttpContext.Session.GetString("Token");
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await Client.GetAsync(ApiUrl + "CarList");
                if (response.IsSuccessStatusCode)
                {
                    var strData = await response.Content.ReadAsStringAsync();

                    var listCar = JsonSerializer.Deserialize<List<CarViewModels>>(strData);
                    if (listCar != null)
                    {
                        Car = listCar;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No Car Found");
                    }
                }
                return Page();
            }
        }
    }
}
