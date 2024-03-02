using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using Repositories;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BusinessObject.Models.UserModels;
using System.Net.Security;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace CarRenting_Client.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient Client;
        private string ApiUrl = "http://localhost:5209/api/Users/";

        public LoginModel()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            Client = new HttpClient(handler);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            Client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("token") == null)
            {
                return Page();
            }
            else
            {
                return RedirectToPage("./Index");
            }
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string token = null;
                HttpResponseMessage response = await Client.GetAsync(ApiUrl + "Login" + "?email=" + Email + "&&password=" + Password);
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("token", response.ToString());
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Wrong email or password!");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
