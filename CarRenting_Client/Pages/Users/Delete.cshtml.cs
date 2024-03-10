using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CarRenting_Client.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient Client;
        private string ApiUrl = "http://localhost:5209/api/Users/";

        public DeleteModel()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            Client = new HttpClient(handler);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            Client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        [BindProperty]
        public UserDisplayDTO User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            /*var role = HttpContext.Session.GetString("RoleID");
            if (role == null || role != "2")
            {
                return RedirectToPage("/Login");
            }
            else
            {*/
            HttpResponseMessage response = await Client.GetAsync(ApiUrl + "GetUser/" + id);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (strData.Equals("\"List is empty!\""))
            {
                User = null;
                return NotFound();
            }
            else
            {
                User = System.Text.Json.JsonSerializer.Deserialize<UserDisplayDTO>(strData, options);
                return Page();
            }
            /*}*/

        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            HttpResponseMessage response = await Client.DeleteAsync(ApiUrl + "Delete/" + id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            return NotFound();
        }
    }
}
