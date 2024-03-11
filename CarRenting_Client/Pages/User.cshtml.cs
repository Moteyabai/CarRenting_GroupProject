using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CarRenting_Client.Pages
{
    public class UserModel : PageModel
    {
        private readonly HttpClient Client;
        private string ApiUrl = "http://localhost:5209/api/Users/";

        public UserModel()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            Client = new HttpClient(handler);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            Client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public IList<UserDisplayDTO> User { get; set; } = default!;
        [BindProperty]
        public string Search { get; set; }
        [BindProperty]
        public int UserID { get; set; }

        public async Task OnGetAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            var role = HttpContext.Session.GetInt32("RoleID");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (token == null && role != 1)
            {
                RedirectToPage("/Login");
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
                    User = System.Text.Json.JsonSerializer.Deserialize<List<UserDisplayDTO>>(strData, options);

                }
            }

        }

        public async Task OnPostAsync()
        {
            /*string json = JsonConvert.SerializeObject(Search);*/
            var token = HttpContext.Session.GetString("Token");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (token == null)
            {
                RedirectToPage("/Login");
            }
            HttpResponseMessage response = await Client.GetAsync(ApiUrl + "Search/" + Search);

            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                User = System.Text.Json.JsonSerializer.Deserialize<List<UserDisplayDTO>>(strData, options);

                RedirectToPage("/User");
            }
            RedirectToPage("/Users");

        }

        public async Task<IActionResult> OnUpdateAsync()
        {
            string json = JsonConvert.SerializeObject(User);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PutAsync(ApiUrl + "Update", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            return BadRequest();
        }

    }
}
