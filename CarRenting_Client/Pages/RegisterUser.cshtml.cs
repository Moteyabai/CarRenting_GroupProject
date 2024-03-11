using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using BusinessObject.DTO;

namespace CarRenting_Client.Pages
{
    public class RegisterUserModel : PageModel
    {
        private string ApiUrl = "http://localhost:5209/api/Users/Register";

        public RegisterUserModel()
        {
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("ID") == null)
            {
                return Page();
            }
            else
            {
                if (HttpContext.Session.GetString("RoleID") == ((int)BusinessObject.Models.Enum.Role.Customer).ToString())
                {
                    return RedirectToPage("./Car");
                }
                else if (HttpContext.Session.GetString("RoleID") == ((int)BusinessObject.Models.Enum.Role.Admin).ToString())
                {
                    return RedirectToPage("./User");
                }
                else if (HttpContext.Session.GetString("RoleID") == ((int)BusinessObject.Models.Enum.Role.Manager).ToString())
                {
                    return RedirectToPage("/Car/Index");
                }
                return Page();
            }
        }

        [BindProperty]
        public UserRegisterDTO User { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (HttpClient Client = new HttpClient())
                {
                    HttpResponseMessage response = await Client.PostAsJsonAsync(ApiUrl, User);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("./Login");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Register fail!");
                        return Page();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
