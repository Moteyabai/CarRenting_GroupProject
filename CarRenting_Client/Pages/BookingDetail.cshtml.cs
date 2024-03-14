using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Models.Enum;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace CarRenting_Client.Pages
{
    public class BookingDetailModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/Booking?$expand=BookingDetails&$filter=BookingID eq ";

        [BindProperty]
        public BookingViewDto Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(int bookingID)
        {
            if (HttpContext.Session.GetString("RoleID") == null)
            {
                return RedirectToPage("./Login");
            }
            else
            {
                string token = HttpContext.Session.GetString("Token");
                List<BookingViewDto> bookingViews = null;
                using (var httpClient = new HttpClient())
                {
                    string apiUrl1 = $"{apiUrl}{bookingID}";
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    using (HttpResponseMessage response = await httpClient.GetAsync(apiUrl1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var apiResponseObject = JObject.Parse(apiResponse);

                        if (apiResponseObject["value"] is JArray bookingReservationsArray)
                        {
                            bookingViews = JsonConvert.DeserializeObject<List<BookingViewDto>>(bookingReservationsArray.ToString())!;
                            // Deserialize as a list if it's an array

                        }
                        Booking = bookingViews.FirstOrDefault();
                        HttpContext.Session.SetInt32("DetailsID", Booking.BookingID);
                    }
                }

                // Now 'booking' contains the deserialized JSON data
                return Page();
            }
        }
    }

    
}
