using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml.Linq;
using BusinessObject;

namespace CarRenting_Client.Pages
{
    public class BookingModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/Booking?$filter=UserID eq 1";

        public List<Booking> Bookings { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            using (var httpClient = new HttpClient())
            {
                // Append the search parameter to the API URL if a name is provided
                string url = apiUrl;

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
