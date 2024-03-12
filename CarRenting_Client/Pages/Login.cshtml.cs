using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
                    return RedirectToPage("/Car");
                }
                else if (HttpContext.Session.GetString("RoleID") == ((int)BusinessObject.Models.Enum.Role.Admin).ToString())
                {
                    return RedirectToPage("/User");
                }
                else if (HttpContext.Session.GetString("RoleID") == ((int)BusinessObject.Models.Enum.Role.Manager).ToString())
                {
                    return RedirectToPage("/Car");
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

                        if (HttpContext.Session.GetString("RoleID") == ((int)BusinessObject.Models.Enum.Role.Customer).ToString())
                        {
                            return RedirectToPage("/Car");
                        }
                        else if (HttpContext.Session.GetString("RoleID") == ((int)BusinessObject.Models.Enum.Role.Admin).ToString())
                        {
                            return RedirectToPage("/User");
                        }
                        else if (HttpContext.Session.GetString("RoleID") == ((int)BusinessObject.Models.Enum.Role.Manager).ToString())
                        {
                            return RedirectToPage("/Car");
                        };


                        return RedirectToPage("/Car");
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
