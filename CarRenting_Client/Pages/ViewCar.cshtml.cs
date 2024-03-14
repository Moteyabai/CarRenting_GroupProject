using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Models.CarModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CarRenting_Client.Pages
{
    public class ViewCarModel : PageModel
    {
        private readonly string apiUrlId = "http://localhost:5209/api/Cars/GetCar";
        private readonly string apiUrlBrands = "http://localhost:5209/api/CarBrands/CarBrandlist";
        private readonly string apiUrlUpdateCar = "http://localhost:5209/api/Cars/UpdateCar";
        private readonly string apiUrlFirebase = "http://localhost:5209/api/Firebase";


        [BindProperty]
        public CarViewModels Cars { get; set; }

        [BindProperty]
        public List<BrandCarDTO> BrandCars { get; set; }

        [BindProperty]
        public IFormFile File { get; set; }

        public async Task<IActionResult> OnGetAsync(int CarID)
        {
            if (HttpContext.Session.GetString("RoleID") == null)
            {
                return RedirectToPage("./Login");
            }
            else
            {
                string token = HttpContext.Session.GetString("Token");
                using (var httpClient = new HttpClient())
                {
                    // Append the search parameter to the API URL if a name is provided
                    string url = $"{apiUrlId}{CarID}";
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    using (HttpResponseMessage response = await httpClient.GetAsync(url))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Cars = JsonConvert.DeserializeObject<CarViewModels>(apiResponse);
                        await LoadRoomTypesAsync();
                        return Page();
                    }
                }
            }
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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                string image;
                if (File != null && File.Length > 0)
                {
                    image = await UploadImage();
                    Cars.ImageCar = image;
                }
                using (var httpClient = new HttpClient())
                {
                    // Send a DELETE request to the API to delete the customer
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{apiUrlUpdateCar}", Cars);

                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful deletion
                        await OnGetAsync(Cars.CarID);
                        return Page();
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        return BadRequest($"Failed to update customer: {errorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
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
    }
}
