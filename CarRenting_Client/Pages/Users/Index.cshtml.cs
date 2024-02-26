using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using System.Net.Http.Headers;
using System.Text.Json;
using BusinessObject.DTO;

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

        public async Task OnGetAsync()
        {
            HttpResponseMessage response = await Client.GetAsync(ApiUrl + "UserList");

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            if (strData.Equals("\"List is empty!\""))
            {
                User = null;
            }
            else
            {
                User = System.Text.Json.JsonSerializer.Deserialize<List<UserDisplayDTO>>(strData, options);
            }
            
        }
    }
}
