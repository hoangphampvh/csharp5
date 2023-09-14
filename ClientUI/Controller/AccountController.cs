using ASMC5.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;
using JsonSerializer = System.Text.Json.JsonSerializer;
using ASMC5.ViewModel;
using Newtonsoft.Json;
using System.Security.Principal;
using BILL.ViewModel.Cart;
using System.Text;

namespace ASMC5.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly HttpClient _httpClient;
        public const string key = "User";
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContext = httpContextAccessor.HttpContext;
            _httpClient = httpClient;
        }

       
        [AllowAnonymous]
        public IActionResult LoginWithGoogle()
        {
            return View();
        }

        public IActionResult GoogleSignIn()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("CallbackGoogle", "Account"),
                Items =
                {
                    { "Scheme", "Google" }
                }
            };

            return Challenge(properties, "Google");
        }
        //[HttpGet("login/OnGetCallbackAsync")]
        User user = new User(); 
        public async Task<IActionResult> CallbackGoogle()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Google");
            if (authenticateResult.Succeeded)
            {
                var accessToken = authenticateResult.Properties.GetTokens().FirstOrDefault(token => token.Name == "access_token")?.Value;
                // var userInfo =await GoogleLoginAsync(token);
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = string.Join("", authenticateResult.Principal.FindFirstValue(ClaimTypes.Name).Replace(" ", "").Split()),
                    Email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email),
                    Password = "Clinet123@$",
                    // Các thuộc tính khác của người dùng
                };
                user = newUser;
                var checkUser = await _userManager.FindByEmailAsync(newUser.Email);

                if (checkUser == null)
                {
                    //// Lưu thông tin người dùng vào bảng UserIdentity
                    var result = await _userManager.CreateAsync(newUser, newUser.Password);
                    checkUser = await _userManager.FindByEmailAsync(newUser.Email);

                    ////    // Xử lý khi tài khoản người dùng được tạo thành công
                    ////    // Ví dụ: Chuyển hướng đến trang chính, gửi email xác nhận, v.v.                 
                    if (result.Succeeded)
                    {

                        var identityUserSignUp = await getRoleAndClaims(checkUser);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identityUserSignUp));
                        // luu thong tin nguoi dung vao cookies
                        _httpContext.Session.SetString(key, JsonSerializer.Serialize(newUser));
                        await CreateCart();

                        return RedirectToAction(nameof(NotNulls));
                    }
                    return BadRequest("false");
                }
                var identityUserSignIn = await getRoleAndClaims(checkUser);
                var principal = new ClaimsPrincipal(identityUserSignIn);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                          principal, new AuthenticationProperties() { IsPersistent = false });
                _httpContext.Session.SetString(key, JsonSerializer.Serialize(checkUser));
                    await CreateCart();

                return RedirectToAction(nameof(NotNulls));
            }
            return RedirectToAction(nameof(nulls));

        }
        public async Task<bool> CreateCart()
        {
            var cartVM = new CartVN();
            cartVM.Status = 0;
            cartVM.Description = "Giỏ Hàng";
            cartVM.UserId = user.Id;
            var CartJSON = JsonConvert.SerializeObject(cartVM);

            // Convert to string content
            var stringContent = new StringContent(CartJSON, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"https://localhost:7257/api/Cart/CreatCart", stringContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }return false;
        }
        public async Task<ClaimsIdentity> getRoleAndClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                await _userManager.AddToRoleAsync(user, "CLIENT");
            }
            var claims = new List<Claim>();
            roles = await _userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));

            }
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return identity;
        }

        // lấy thông tin người dùng từ token
        //public async Task<GoogleUserInfoVM> GoogleLoginAsync(string token)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        var response = await client.GetAsync("https://account.google.com/o/oauth2/v3/auth");
        //        response.EnsureSuccessStatusCode();
        //        var content = await response.Content.ReadAsStringAsync();
        //        var userInfo = JsonConvert.DeserializeObject<GoogleUserInfoVM>(content);
        //        return userInfo;
        //    }
        //}

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [Authorize]
        public IActionResult NotNulls()
        {
            // Kiểm tra xem mã token có tồn tại và hợp lệ không
            if (User.Identity !=null && User.Identity.IsAuthenticated)
            {
                // Mã token hợp lệ, thực hiện các logic của action NotNulls
                return View();
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return BadRequest("dell");
            }
        }


        public IActionResult nulls()
        {
            return View();
        }

    }
}
