﻿using System;
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
                return RedirectToPage("./Car");
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
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
                if (Email == configuration["Admin:email"] && Password == configuration["Admin:password"])
                {
                    HttpContext.Session.SetString("ID", "ADMIN");
                    HttpContext.Session.SetString("userName", "ADMIN");
                    HttpContext.Session.SetString("email", "ADMIN");
                    HttpContext.Session.SetString("RoleID", "ADMIN");
                    return RedirectToPage("./Car");
                }
                else
                {
                    HttpClient Client = new HttpClient();
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