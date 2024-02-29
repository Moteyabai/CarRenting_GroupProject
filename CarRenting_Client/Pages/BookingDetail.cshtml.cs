using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using BusinessObject;
using BusinessObject.DTO;

namespace CarRenting_Client.Pages
{
    public class BookingDetailModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/Booking?$expand=BookingDetails&$filter=BookingID eq ";

        [BindProperty]
        public BookingViewDto Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(int bookingID)
        {
            BookingViewDto booking = null;
            using (var httpClient = new HttpClient())
            {
                string apiUrl1 = $"{apiUrl}{bookingID}";

                using (HttpResponseMessage response = await httpClient.GetAsync(apiUrl1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    booking = JsonConvert.DeserializeObject<BookingViewDto>(apiResponse);
                }
            }

            // Now 'booking' contains the deserialized JSON data
            return Page();
        }
    }

    
}
