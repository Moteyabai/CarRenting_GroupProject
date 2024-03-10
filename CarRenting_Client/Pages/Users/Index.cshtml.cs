using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using System.Net.Http.Headers;
using System.Text.Json;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CarRenting_Client.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient Client;
        private string ApiUrl = "http://localhost:5209/api/Users/";

        public IndexModel()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            Client = new HttpClient(handler);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            Client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public IList<UserDisplayDTO> User { get;set; } = default!;
        [BindProperty]
        public string Search {  get; set; }

        public async Task OnGetAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await Client.GetAsync(ApiUrl + "UserList");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                User = System.Text.Json.JsonSerializer.Deserialize<List<UserDisplayDTO>>(strData, options);

                RedirectToPage("./Index");
            }
            RedirectToPage("./Index");
        }

        public async Task OnPostAsync()
        {
            /*string json = JsonConvert.SerializeObject(Search);*/;
            HttpResponseMessage response = await Client.GetAsync(ApiUrl + "Search/" + Search);

            if(response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions {       
                PropertyNameCaseInsensitive = true,
            };
                User = System.Text.Json.JsonSerializer.Deserialize<List<UserDisplayDTO>>(strData, options);
 
            RedirectToPage("./Index");
            }
            RedirectToPage("./Index");

        }
            
    }
}
