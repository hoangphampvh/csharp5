using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViewModel.ViewModel;
using BILL.ViewModel.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using NuGet.Common;
using Newtonsoft.Json.Linq;

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
        public async Task<IActionResult> Login(LoginRequestVM loginUser)
        {

            //if (User.Identity != null && User.Identity.IsAuthenticated)
            //{
            //    // Người dùng đã đăng nhập, chuyển hướng đến trang khác
            //    return RedirectToAction("Index", "Home");
            //}
            //else
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
                    if (token == "") return View();
                    HttpContext.Session.SetString("JWTToken", token);
                    var handler = new JwtSecurityTokenHandler();
                    if (handler == null) return View();
                    var jwt = handler.ReadJwtToken(token);
                    if (jwt == null) return View();

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name).Value));

                    // Lấy danh sách roles từ JWT
                    var roles = jwt.Claims.Where(u => u.Type == ClaimTypes.Role).Select(u => u.Value).ToList();

                    // Thêm các roles vào danh tính
                    foreach (var role in roles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                         principal, new AuthenticationProperties() { IsPersistent = loginUser.RememberMe });
                      return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JWTToken");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
