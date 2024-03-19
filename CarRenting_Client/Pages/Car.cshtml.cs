using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Models.CarModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace CarRenting_Client.Pages.Users
{
    public class CarModel : PageModel
    {

        private readonly string apiUrl = "http://localhost:5209/api/Cars/Carlist";
        private readonly string apiUrlSearch = "http://localhost:5209/api/Cars/Search/";
        private readonly string apiUrlId = "http://localhost:5209/api/Cars/GetCar";
        private readonly string apiUrlBooking = "http://localhost:5209/odata/Booking";
        private readonly string apiUrlBrands = "http://localhost:5209/api/CarBrands/CarBrandlist";
        private readonly string apiUrlAddCars = "http://localhost:5209/api/Cars/AddCar";
        private readonly string apiUrlFirebase = "http://localhost:5209/api/Firebase";

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

        public CarViewModels Car { get; set; }

        [BindProperty]
        public List<BrandCarDTO> BrandCars { get; set; }

        [BindProperty]
        public CarAddDTO RoomInformationDto { get; set; }

        [BindProperty]
        public IFormFile File { get; set; }

        public List<CarViewModels> Cars { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string token = HttpContext.Session.GetString("Token");
            using (var httpClient = new HttpClient()) {
                // Append the search parameter to the API URL if a name is provided
                string url = string.IsNullOrEmpty(Name) ? apiUrl : $"{apiUrlSearch}{Name}";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (HttpResponseMessage response = await httpClient.GetAsync(url)) {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode) {
                        Cars = JsonConvert.DeserializeObject<List<CarViewModels>>(apiResponse);
                    }
                    else {
                        Cars = null;
                    }
                }
            }

            // Filter the list of cars based on user role and car status
            if (HttpContext.Session.GetString("RoleID") != null) {
                string roleID = HttpContext.Session.GetString("RoleID");

                // If the user is role 1 or 3, filter cars with status 1
                if (roleID.Equals("1") || roleID.Equals("3")) {
                    Cars = Cars.Where(car => car.Status == 1).ToList();
                }
                // If the user is role 2, show all cars
                else if (roleID.Equals("2")) {
                    // Do nothing, show all cars regardless of status
                }
            }

            if (HttpContext.Session.GetString("ID") == null) {
                return RedirectToPage("./Login");
            }
            int[] seatNumbers = new int[] { 4, 7, 16 };
            ViewData["SeatNumbers"] = seatNumbers.Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();
            await LoadRoomTypesAsync();
            return Page();
        }


        private async Task LoadRoomTypesAsync()
        {
            string token = HttpContext.Session.GetString("Token");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync(apiUrlBrands);

                if (response.IsSuccessStatusCode)
                {
                    var roomTypesJson = await response.Content.ReadAsStringAsync();
                    BrandCars = JsonConvert.DeserializeObject<List<BrandCarDTO>>(roomTypesJson);

                    // Use ViewData to store the SelectList for the dropdown
                    ViewData["BrandCar"] = new SelectList(BrandCars, "CarBrandID", "Name");
                }
                else
                {
                    // Handle error, e.g., log or display a message
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(int CarID)
        {
            try
            {
                string url = apiUrlId;
                string userIDString = HttpContext.Session.GetString("ID");
                int userID = int.Parse(userIDString);
                string token = HttpContext.Session.GetString("Token");
                using (var httpClient = new HttpClient()) // Declare and initialize httpClient
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.GetAsync($"{url}{CarID}");
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Car = JsonConvert.DeserializeObject<CarViewModels>(apiResponse);
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
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

        public async Task<IActionResult> OnPostCreateRoomInformationAsync()
        {
            try {
                string token = HttpContext.Session.GetString("Token");
                string image;
                if (File != null && File.Length > 0) {
                    image = await UploadImage();
                    RoomInformationDto.ImageCar = image;
                }
                using (var httpClient = new HttpClient()) {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    // Send a POST request to the API to create room information
                    var response = await httpClient.PostAsJsonAsync(apiUrlAddCars, RoomInformationDto);

                    if (response.IsSuccessStatusCode) {
                        // Reload the page after successful creation
                        TempData["Message"] = "Add Car Successed.";
                        return RedirectToPage();
                    }
                    else {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == HttpStatusCode.BadRequest) {
                            TempData["Message"] = "Car Plate Exists.";
                            return Page();
                        }
                        return Page();
                    }
                }
            }
            catch (Exception ex) {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        private async Task<string> UploadImage()
        {
            string token = HttpContext.Session.GetString("Token");
            if (File != null && File.Length > 0)
            {
                using (var httpClient = new HttpClient())
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StreamContent(File.OpenReadStream()), "stream", File.FileName);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrlFirebase, formData);

                    if (response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        return errorMessage;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("./Login");
        }
        public async Task<IActionResult> OnPostDeleteAsync(int carID)
        {
            try {
                string token = HttpContext.Session.GetString("Token");
                using (var httpClient = new HttpClient()) {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.DeleteAsync($"http://localhost:5209/api/Cars/DeleteCar/{carID}");

                    if (response.IsSuccessStatusCode) {
                        TempData["Message"] = "Car successfully deleted.";
                    }
                    else {
                        TempData["Message"] = "The car's status has changed to 0.";
                    }
                }
            }
            catch (Exception ex) {
                TempData["Message"] = "An error occurred while deleting the car.";
            }

            return RedirectToPage();
        }
       


    }
}
