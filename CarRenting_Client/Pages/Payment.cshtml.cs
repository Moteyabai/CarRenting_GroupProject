using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CarRenting_Client.Pages
{
    public class PaymentModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/Transaction?";

        public List<TransactionViewDTO> Transactions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("RoleID") == null)
            {
                return RedirectToPage("./Login");
            }
            else
            {
                string token = HttpContext.Session.GetString("Token");
                using (var httpClient = new HttpClient())
                {
                    // Append the search parameter to the API URL if a name is provided
                    string url = apiUrl;
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    using (HttpResponseMessage response = await httpClient.GetAsync(url))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var roomArray = JObject.Parse(apiResponse)["value"];

                        if (roomArray is JArray)
                        {
                            // Deserialize as a list if it's an array
                            Transactions = JsonConvert.DeserializeObject<List<TransactionViewDTO>>(roomArray.ToString())!;
                        }
                    }
                }

                return Page();
            }
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("./Login");
        }
    }
}
