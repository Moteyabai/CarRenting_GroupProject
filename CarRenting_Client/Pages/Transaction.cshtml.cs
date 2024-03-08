using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace CarRenting_Client.Pages
{
    public class TransactionModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/Transaction?$filter=UserID eq ";

        public List<TransactionViewDTO> Transactions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string userID = HttpContext.Session.GetString("ID");
            using (var httpClient = new HttpClient())
            {
                // Append the search parameter to the API URL if a name is provided
                string url = $"{apiUrl}{userID}";

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
}
