using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViewModel.ViewModel;

namespace ASMC5.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            // Convert registerUser to JSON
            var loginUserJSON = JsonConvert.SerializeObject(loginUser);

            // Convert to string content
            var stringContent = new StringContent(loginUserJSON, Encoding.UTF8, "application/json");

            // Send request POST to register API
            var response = await _httpClient.PostAsync($"https://localhost:7257/api/Users/Login", stringContent);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                HttpContext.Session.SetString("JWTToken", token);               
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = await response.Content.ReadAsStringAsync();
                return View();
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWTToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
