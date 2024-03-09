using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Models.CarModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace CarRenting_Client.Pages.Users
{
    public class CarModel : PageModel
    {

        private readonly string apiUrl = "http://localhost:5209/api/Cars/Carlist";
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

        public List<CarViewModels> Cars { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            using (var httpClient = new HttpClient())
            {
                // Append the search parameter to the API URL if a name is provided
                string url = string.IsNullOrEmpty(Name) ? apiUrl : $"{apiUrlSearch}{Name}";

                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Cars = JsonConvert.DeserializeObject<List<CarViewModels>>(apiResponse);
                }
            }

            if (HttpContext.Session.GetString("ID") == null)
            {
                return RedirectToPage("./Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int CarID)
        {
            try
            {
                string url = apiUrlId;
                string userIDString = HttpContext.Session.GetString("ID");
                int userID = int.Parse(userIDString);

                using (var httpClient = new HttpClient()) // Declare and initialize httpClient
                {
                    HttpResponseMessage response = await httpClient.GetAsync($"{url}{CarID}");
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Car = JsonConvert.DeserializeObject<Car>(apiResponse);
                }

                var bookingDetail = new BookingDetailDTO
                {
                    CarID = CarID,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    CarStatus = "1",
                    CarPrice = Car.PricePerDay,
                    Fined = 0
                };
                var booking = new BookingDTO
                {
                    UserID = userID,
                    BookingDetails = new List<BookingDetailDTO> { bookingDetail }, // Initialize as a list and add bookingDetail to it
                };
                Booking = booking;
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(apiUrlBooking, Booking);

                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful deletion
                        TempData["Message"] = "Booking successfully created.";
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        JObject errorObject = JObject.Parse(errorMessage);

                        // Lấy thông báo từ thuộc tính "message" trong đối tượng error
                        string message = (string)errorObject["error"]["message"];

                        // Lưu thông báo vào TempData
                        TempData["Message"] = message;
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Date validation.";
            }

            return RedirectToPage("Car"); // Trả về trang hiện tại sau khi đã xử lý các ngoại lệ
        }

    }
}
