using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CarRenting_Client.Pages
{
    public class StaffBookingModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/Booking?$expand=User";
        private readonly string apiSearch = "http://localhost:5209/odata/Booking?$expand=User&$filter=contains(tolower(User/UserName), '";

        public List<Booking> Bookings { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            using (var httpClient = new HttpClient())
            {
                string url = string.IsNullOrEmpty(Name) ? apiUrl : $"{apiSearch}{Name}')";

                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var roomArray = JObject.Parse(apiResponse)["value"];

                    if (roomArray is JArray)
                    {
                        // Deserialize as a list if it's an array
                        Bookings = JsonConvert.DeserializeObject<List<Booking>>(roomArray.ToString())!;
                    }
                }
            }


            return Page();
        }
    }
}
