using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace CarRenting_Client.Pages
{
    public class PaymentSuccessModel : PageModel
    {
        private readonly string apiUrlCreatePayment = "http://localhost:5209/odata/Transaction";
        private readonly string apiUrlAccept = "http://localhost:5209/odata/Booking/";

        [BindProperty]
        public BookingUpdateDTO BookingUpdate { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string userIDString = HttpContext.Session.GetString("ID");
            int userID = int.Parse(userIDString);
            int total = HttpContext.Session.GetInt32("Total")??0;
            string token = HttpContext.Session.GetString("Token");
            var paymentDto = new TransactionDTO
            {
                UserID = userID,
                Price = total,
            };
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                // Send a POST request to the API to create room information
                var response = await httpClient.PostAsJsonAsync(apiUrlCreatePayment, paymentDto);

                if (response.IsSuccessStatusCode)
                {
                    await OnPostFinishAsync();
                    return Page();
                }
                else
                {
                    return Page();
                }
            }
        }


        public async Task<IActionResult> OnPostFinishAsync()
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                int? contractID = HttpContext.Session.GetInt32("DetailsID");
                BookingUpdate = new BookingUpdateDTO { Status = 4 };
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{apiUrlAccept}{contractID}", BookingUpdate);

                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful deletion
                        return Page();
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        return BadRequest($"Failed to update booking: {errorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update booking: {ex.Message}");
            }
        }


        public IActionResult OnPost()
        {
            return RedirectToPage("Car");
        }

    }
}
