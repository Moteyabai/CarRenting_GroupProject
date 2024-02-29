using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BusinessObject;

namespace CarRenting_Client.Pages
{
    public class CarDamageModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/CarDamage?$filter=BookingDetailID eq ";

        [BindProperty]
        public CarDamage CarDamage { get; set; }

        public async Task<IActionResult> OnGetAsync(int damageID)
        {
            List<CarDamage> roomInformations = null;
            using (var httpClient = new HttpClient())
            {
                // Append the search parameter to the API URL if a name is provided
                string url = $"{apiUrl}{damageID}";

                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var roomArray = JObject.Parse(apiResponse)["value"];

                    if (roomArray is JArray)
                    {
                        // Deserialize as a list if it's an array
                        roomInformations = JsonConvert.DeserializeObject<List<CarDamage>>(roomArray.ToString())!;
                    }
                    CarDamage = roomInformations.FirstOrDefault();
                    return Page();
                }
            }

        }
    }
}
