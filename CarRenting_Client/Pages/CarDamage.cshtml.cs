using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BusinessObject;
using BusinessObject.Models;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace CarRenting_Client.Pages
{
    public class CarDamageModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/CarDamage?$expand=BookingDetail&$filter=BookingDetailID eq ";
        private readonly string apiUrlUpdate = "http://localhost:5209/odata/CarDamage/";
        private readonly string apiUrlBooking = "http://localhost:5209/odata/Booking?$filter=BookingID eq ";
        private readonly string apiUrlCreatePayment = "http://localhost:5209/odata/Transaction";
        private readonly string apiUrlFirebase = "http://localhost:5209/api/Firebase";
        private readonly StripeSettings _stripeSettings;
        public CarDamageModel(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
        }

        [BindProperty]
        public CarDamage CarDamage { get; set; }

        [BindProperty]
        public PaymentDTO PaymentDTO { get; set; }

        [BindProperty(SupportsGet = true)]
        public string amount { get; set; }

        [BindProperty(SupportsGet = true)]
        public BookingPayDTO Booking { get; set; }

        [BindProperty]
        public IFormFile File { get; set; }


        public async Task<IActionResult> OnGetAsync(int damageID)
        {
            string token = HttpContext.Session.GetString("Token");
            List<CarDamage> roomInformations = null;
            using (var httpClient = new HttpClient())
            {
                // Append the search parameter to the API URL if a name is provided
                string url = $"{apiUrl}{damageID}";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

                    //get bookingID



                    await LoadAsync(CarDamage.BookingDetail.BookingID, CarDamage.Fined);
                    return Page();
                }
            }
            

        }

        private async Task LoadAsync(int bookingID, decimal fined)
        {
            string token = HttpContext.Session.GetString("Token");
            using (var httpClient = new HttpClient())
            {
                // Append the search parameter to the API URL if a name is provided
                string url = $"{apiUrlBooking}{bookingID}";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var roomArray = JObject.Parse(apiResponse)["value"];

                    if (roomArray is JArray)
                    {
                        // Deserialize as a list if it's an array
                        var bookings = JsonConvert.DeserializeObject<List<BookingPayDTO>>(roomArray.ToString());
                        if (bookings.Count > 0)
                        {
                            Booking = bookings[0];
                        }
                        else
                        {
                            // Xử lý trường hợp không có dữ liệu trả về
                            throw new Exception("Không có dữ liệu Booking trả về từ API.");
                        }
                    }

                }
                PaymentDTO = new PaymentDTO
                {
                    booking = Booking.TotalPrice,
                    damage = fined,
                    total = Booking.TotalPrice + fined,
                    Status = Booking.Status,
                };
                HttpContext.Session.SetInt32("Total", (int)PaymentDTO.total);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string token = HttpContext.Session.GetString("Token");
            try
            {
                string image;
                if (File != null && File.Length > 0)
                {
                    image = await UploadImage();
                    CarDamage.ImageCarDamage = image;
                }
                
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{apiUrlUpdate}{CarDamage.CarDamageID}", CarDamage);

                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful deletion
                        TempData["Message"] = "update car damage successful.";
                        return RedirectToPage("StaffBooking");
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        TempData["Message"] = "update car damage fail.";
                        return BadRequest($"Failed to update car damage: {errorMessage}");
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public IActionResult OnPostCreateCheckOutSession()
        {
            var currency = "vnd";
            var successUrl = "http://localhost:5049/PaymentSuccess";
            var cancelUrl = "http://localhost:5049/PaymentFail";
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            var option = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions> {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = currency,
                            UnitAmount = Convert.ToInt32(PaymentDTO.total)*100,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Contract hire car",
                                Description = "Description detail"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            var session = service.Create(option);

            return Redirect(session.Url);
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

    }
}
