﻿using BILL.ViewModel.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MudBlazor;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using BLL.ViewModel.ModelConfiguration;

namespace ClientUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;

        }
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl = "/")
        {
            var jwtToken = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
            LoginRequestVM objLoginModel = new LoginRequestVM();
            objLoginModel.ReturnUrl = ReturnUrl;          
            if (jwtToken == null)
            {
                return View(objLoginModel);
            }
            return RedirectToPage("/_Host");
        }
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithJWT(LoginRequestVM loginRequest)
        {
            var result = await _httpClient.PostAsJsonAsync("/api/Account/Login", loginRequest);
            if (result.IsSuccessStatusCode)
            {
                var token = await result.Content.ReadAsStringAsync();
                var objToken = JsonConvert.DeserializeObject<LoginResponesVM>(token);
                if (objToken!=null)
                {
                    TokenModel jsonObject = JsonConvert.DeserializeObject<TokenModel>(objToken.Data.ToString());
                    if(jsonObject != null)
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("_tokenAuthorization", jsonObject.AccessToken);
                        var handler = new JwtSecurityTokenHandler();

                        var jwt = handler.ReadJwtToken(jsonObject.AccessToken);

                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        var value = jwt.Claims;
                        identity.AddClaims(value);
                       
                   
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonObject.AccessToken);

                        return RedirectToPage("/_Host");
                    }
                    
                }
                return BadRequest("Token is null");
            }

            return RedirectToAction("Login");
        }
        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page
            return LocalRedirect("/");
        }
    }
}
