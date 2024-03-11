using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using Repositories;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BusinessObject.Models.UserModels;
using System.Net.Security;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using BusinessObject.Models.Enum;
using Newtonsoft.Json.Linq;

namespace CarRenting_Client.Pages
{
    public class LoginModel : PageModel
    {
        private string ApiUrl = "http://localhost:5209/api/Users/login";
        
        public LoginModel()
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
                    return RedirectToPage("/Users/Index");
                }
                else if (HttpContext.Session.GetString("RoleID") == ((int)BusinessObject.Models.Enum.Role.Manager).ToString())
                {
                    return RedirectToPage("./Manager_CarList");
                };
                return Page();
            }
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }


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
                    HttpResponseMessage response = await Client.GetAsync(ApiUrl + "?email=" + Email + "&&password=" + Password);
                    if (response.IsSuccessStatusCode)
                    {
                        string token = await response.Content.ReadAsStringAsync();
                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadJwtToken(token);
                        var userId = jsonToken.Claims.First(claims => claims.Type == "userID").Value;
                        var userName = jsonToken.Claims.First(claims => claims.Type == "userName").Value;
                        var email = jsonToken.Claims.First(claims => claims.Type == "email").Value;
                        var roleID = jsonToken.Claims.First(claims => claims.Type == "roleID").Value;
                        HttpContext.Session.SetString("ID", userId);
                        HttpContext.Session.SetString("userName", userName);
                        HttpContext.Session.SetString("email", email);
                        HttpContext.Session.SetString("RoleID", roleID);
                        HttpContext.Session.SetString("Token", token);

                        if (roleID == ((int)BusinessObject.Models.Enum.Role.Admin).ToString())
                        {
                            return RedirectToPage("./User");
                        }
                        else if (roleID == ((int)BusinessObject.Models.Enum.Role.Manager).ToString()) 
                        {
                            return RedirectToPage("./Manager_CarList");
                        }
                        
                        //get role staff
                        string role = HttpContext.Session.GetString("RoleID");
                        int rl = int.Parse(role);
                        if (rl == 3)
                        {
                            return RedirectToPage("./StaffBooking");
                        }
                        return RedirectToPage("./Car");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Wrong email or password!");
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
