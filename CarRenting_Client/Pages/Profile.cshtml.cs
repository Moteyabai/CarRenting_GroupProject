using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Stripe;

namespace CarRenting_Client.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/api/Users/GetUser/";
        private readonly string apiUrlUpdate = "http://localhost:5209/api/Users/Update";

        [BindProperty]
        public User Customer { get; set; }

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
                using (var httpClient = new HttpClient())
                {
                    // Append the search parameter to the API URL if a name is provided
                    string url = $"{apiUrl}{userID}";


                    using (HttpResponseMessage response = await httpClient.GetAsync(url))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Customer = JsonConvert.DeserializeObject<User>(apiResponse);
                    }
                }


                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                else
                {
                    using (var httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.PutAsJsonAsync(apiUrlUpdate, Customer);

                        if (response.IsSuccessStatusCode)
                        {
                            // Reload the page after successful deletion
                            return RedirectToPage();
                        }
                        else
                        {
                            string errorMessage = await response.Content.ReadAsStringAsync();
                            return BadRequest($"Failed to update customer: {errorMessage}");
                        }
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
