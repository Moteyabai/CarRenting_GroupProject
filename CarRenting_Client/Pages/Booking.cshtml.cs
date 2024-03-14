using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml.Linq;
using BusinessObject;
using System.Net.Http.Headers;

namespace CarRenting_Client.Pages
{
    public class BookingModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/Booking?$filter=UserID eq ";

        public List<Booking> Bookings { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("RoleID") == null)
            {
                return RedirectToPage("./Login");
            }
            else
            {
                string userIDString = HttpContext.Session.GetString("ID");
                int userID = int.Parse(userIDString);
                string token = HttpContext.Session.GetString("Token");
                using (var httpClient = new HttpClient())
                {
                    // Append the search parameter to the API URL if a name is provided
                    string url = $"{apiUrl}{userID}";
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
}
