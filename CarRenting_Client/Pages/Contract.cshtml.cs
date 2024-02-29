using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CarRenting_Client.Pages
{
    public class ContractModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/Contract$filter=ContractID eq ";

        [BindProperty]
        public Contract Contract { get; set; }

        public async Task<IActionResult> OnGetAsync(int contractID)
        {
            List<Contract> roomInformations = null;
            using (var httpClient = new HttpClient())
            {
                // Append the search parameter to the API URL if a name is provided
                string url = $"{apiUrl}{contractID}";

                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var roomArray = JObject.Parse(apiResponse)["value"];

                    if (roomArray is JArray)
                    {
                        // Deserialize as a list if it's an array
                        roomInformations = JsonConvert.DeserializeObject<List<Contract>>(roomArray.ToString())!;
                    }
                    Contract = roomInformations.FirstOrDefault();
                    return Page();
                }
            }

        }
    }
}
