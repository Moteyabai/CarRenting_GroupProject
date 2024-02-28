using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;

namespace CarRenting_Client.Pages.Users
{
    public class CarModel : PageModel
    {

        private readonly string apiUrl = "http://localhost:5209/api/Cars/car-list";
        private readonly string apiUrlSearch = "http://localhost:5209/api/Cars/Search/";
        private readonly string apiUrlId = "http://localhost:5209/api/Cars/GetCar";
        private readonly string apiUrlBooking = "http://localhost:5209/odata/Booking";

        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public BookingDTO Booking { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CarID { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; }

        public Car Car { get; set; }

        public List<Car> Cars { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            using (var httpClient = new HttpClient())
            {
                // Append the search parameter to the API URL if a name is provided
                string url = string.IsNullOrEmpty(Name) ? apiUrl : $"{apiUrlSearch}{Name}";

                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Cars = JsonConvert.DeserializeObject<List<Car>>(apiResponse);
                }
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int CarID)
        {
            try
            {
                string url = apiUrlId;

                using (var httpClient = new HttpClient()) // Declare and initialize httpClient
                {
                    HttpResponseMessage response = await httpClient.GetAsync($"{url}{CarID}");
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Car = JsonConvert.DeserializeObject<Car>(apiResponse);
                }

                var bookingDetail = new BookingDetailDTO
                {
                    CarID = CarID,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    CarStatus = "1",
                    CarPrice = Car.PricePerDay,
                    Fined = 0
                };
                var booking = new BookingDTO
                {
                    UserID = 1,
                    BookingDetails = new List<BookingDetailDTO> { bookingDetail }, // Initialize as a list and add bookingDetail to it
                };
                Booking = booking;
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(apiUrlBooking, Booking);

                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful deletion
                        return RedirectToPage();
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        return BadRequest($"Failed to booking: {errorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
