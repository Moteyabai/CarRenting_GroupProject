﻿using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BusinessObject.DTO;
using System.Net.Http.Headers;
using writehtml;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRenting_Client.Pages
{
    public class ContractModel : PageModel
    {
        private readonly string apiUrl = "http://localhost:5209/odata/Contract?$filter=ContractID eq ";
        private readonly string apiUrlAccept = "http://localhost:5209/odata/Booking/";
        private readonly string apiUrlReject = "http://localhost:5209/odata/Booking/";
        private readonly string apiUrlUpdate = "http://localhost:5209/odata/Contract/";
        private readonly string apiUrlGetBoking = "http://localhost:5209/odata/BookingDetail?$filter=ContractID eq ";
        private readonly string apiUrlDetail = "http://localhost:5209/odata/Booking?$expand=BookingDetails($expand=Car($expand=CarBrand)),User&$filter=BookingID eq ";

        [BindProperty]
        public Contract Contract { get; set; }

        [BindProperty]
        public BookingDetailViewDTO Booking1 { get; set; }

        [BindProperty]
        public BookingDetailsDto Booking { get; set; }

        [BindProperty]
        public BookingUpdateDTO BookingUpdate { get; set; }

        public async Task<IActionResult> OnGetAsync(int contractID)
        {
            if (HttpContext.Session.GetString("RoleID") == null)
            {
                return RedirectToPage("./Login");
            }
            else
            {
                string token = HttpContext.Session.GetString("Token");
                List<Contract> roomInformations = null;
                using (var httpClient = new HttpClient())
                {
                    // Append the search parameter to the API URL if a name is provided
                    string url = $"{apiUrl}{contractID}";
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
                        await LoadRoomTypesAsync(contractID);
                        return Page();
                    }
                }
            }
        }

        private async Task LoadRoomTypesAsync(int contractId)
        {
            string token = HttpContext.Session.GetString("Token");
            List<BookingDetailViewDTO> roomInformations = null;
            using (var httpClient = new HttpClient())
            {
                // Append the search parameter to the API URL if a name is provided
                string url = $"{apiUrlGetBoking}{contractId}";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var roomArray = JObject.Parse(apiResponse)["value"];

                    if (roomArray is JArray)
                    {
                        // Deserialize as a list if it's an array
                        roomInformations = JsonConvert.DeserializeObject<List<BookingDetailViewDTO>>(roomArray.ToString())!;
                    }
                    Booking1 = roomInformations.FirstOrDefault();
                    await LoadDetailAsync(Booking1.BookingID);
                }
            }
        }

        private async Task LoadDetailAsync(int bookingID)
        {
            string token = HttpContext.Session.GetString("Token");
            List<BookingDetailsDto> bookingViews = null;
            using (var httpClient = new HttpClient())
            {
                string apiUrl1 = $"{apiUrlDetail}{bookingID}";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using (HttpResponseMessage response = await httpClient.GetAsync(apiUrl1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var apiResponseObject = JObject.Parse(apiResponse);

                    if (apiResponseObject["value"] is JArray bookingReservationsArray)
                    {
                        bookingViews = JsonConvert.DeserializeObject<List<BookingDetailsDto>>(bookingReservationsArray.ToString())!;
                        // Deserialize as a list if it's an array

                    }
                    Booking = bookingViews.FirstOrDefault();
                }
            }
        }

        public async Task<IActionResult> OnPostAcceptAsync(int contractID)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                BookingUpdate = new BookingUpdateDTO { Status = 2 };
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{apiUrlAccept}{contractID}", BookingUpdate);

                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful deletion
                        await LoadAsync(contractID);
                        return RedirectToPage("Booking");
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        TempData["Message"] = "accept contract fail.";
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
                string token = HttpContext.Session.GetString("Token");
                BookingUpdate = new BookingUpdateDTO { Status = 3 };
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{apiUrlReject}{contractID}", BookingUpdate);

                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful deletion
                        await LoadAsync(contractID);
                        return RedirectToPage("Booking");
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

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{apiUrlUpdate}{Contract.ContractID}", Contract);

                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful deletion
                        LoadAsync1(Contract.ContractID);
                        return RedirectToPage("StaffBooking");
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        TempData["Message"] = "update contract fail.";
                        return BadRequest($"Failed to update contract: {errorMessage}");
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        private async Task LoadAsync(int contractID)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                var cotract = new Contract
                {
                    Status = 2
                };
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{apiUrlUpdate}{contractID}", cotract);

                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful deletion
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        
                    }
                }

            }
            catch (Exception ex)
            {
                
            }
        }

        private async Task LoadAsync1(int contractID)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                var cotract = new Contract
                {
                    Status = 3
                };
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{apiUrlUpdate}{contractID}", cotract);

                    if (response.IsSuccessStatusCode)
                    {
                        // Reload the page after successful deletion
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();

                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("./Login");
        }

        public async Task<IActionResult> OnPostExportAsync()
        {
            try
            {
                string htmlContent = Contract.CarInformation;
                string fileName = "HopDongThueXe23111.docx";
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                Writing11.ConvertHtmlToDocx(htmlContent, outputPath);
                Console.WriteLine(outputPath);
                return RedirectToPage("Car");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to export contract: {ex.Message}");
            }
        }
    }
}
