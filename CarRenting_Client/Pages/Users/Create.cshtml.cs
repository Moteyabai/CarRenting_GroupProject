using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using System.Net.Http.Headers;
using BusinessObject.DTO;
using Newtonsoft.Json;
using System.Text;
using NuGet.Protocol.Plugins;

namespace CarRenting_Client.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient Client;
        private string ApiUrl = "http://localhost:5209/api/Users/";

        public CreateModel()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            Client = new HttpClient(handler);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            Client.DefaultRequestHeaders.Accept.Add(contentType);
        }

/*        public IActionResult OnGet()
        {
        ViewData["RoleID"] = new SelectList(_context.Roles, "RoleID", "Name");
            return Page();
        }*/

        [BindProperty]
        public UserRegisterDTO User { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string json = JsonConvert.SerializeObject(User);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PostAsync(ApiUrl + "Register", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            return BadRequest();
        }
    }
}
