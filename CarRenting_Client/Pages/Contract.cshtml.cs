using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BusinessObject.DTO;

namespace CarRenting_Client.Pages
{
    public class ContractModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/Contract?$filter=ContractID eq ";
        private readonly string apiUrlAccept = "http://localhost:5209/odata/Booking/";
        private readonly string apiUrlReject = "http://localhost:5209/odata/Booking/";
        private readonly string apiUrlUpdate = "http://localhost:5209/odata/Contract/";

        [BindProperty]
        public Contract Contract { get; set; }

        [BindProperty]
        public BookingUpdateDTO BookingUpdate { get; set; }

        public async Task<IActionResult> OnGetAsync(int contractID)
        {
            List<Contract> roomInformations = null;
            using (var httpClient = new HttpClient())
            {
                // Append the search parameter to the API URL if a name is provided
                string url = $"{apiUrl}{contractID}";

                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var roomArray = JObject.Parse(apiResponse)["value"];

                    if (roomArray is JArray)
                    {
                        // Deserialize as a list if it's an array
                        roomInformations = JsonConvert.DeserializeObject<List<Contract>>(roomArray.ToString())!;
                    }
                    Contract = roomInformations.FirstOrDefault();
                    return Page();
                }
            }

        }

        public async Task<IActionResult> OnPostAcceptAsync(int contractID)
        {
            try
            {
                BookingUpdate = new BookingUpdateDTO { Status = 2 };
                using (var httpClient = new HttpClient())
                {
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
            catch(Exception ex)
            {
                return BadRequest($"Failed to update booking: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnPostRejectAsync(int contractID)
        {
            try
            {
                BookingUpdate = new BookingUpdateDTO { Status = 3 };
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{apiUrlReject}{contractID}", BookingUpdate);

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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{apiUrlUpdate}{Contract.ContractID}", Contract);

                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful deletion
                        return Page();
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        return BadRequest($"Failed to update contract: {errorMessage}");
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
