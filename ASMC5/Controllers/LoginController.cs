using ASMC5.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Principal;

namespace ASMC5.Controllers
{
    public class loginController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        public const string key = "User";
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public loginController(SignInManager<User> signInManager, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [HttpGet("/login/")]
        [AllowAnonymous]
        public IActionResult LoginWithGoogle(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult GoogleSignIn()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(OnGetCallbackAsync)),
                Items =
                {
                    { "Scheme", "Google" }
                }
            };

            return Challenge(properties, "Google");
        }
        [HttpGet("login/OnGetCallbackAsync")]
        public async Task<IActionResult> OnGetCallbackAsync()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Google");
            if (authenticateResult.Succeeded)
            {
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = string.Join("", authenticateResult.Principal.FindFirstValue(ClaimTypes.Name).Replace(" ", "").Split()),
                    Email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email),
                    Password = "Clinet123@$",
                    // Các thuộc tính khác của người dùng
                };
                var checkUser = await _userManager.FindByEmailAsync(newUser.Email);

                if (checkUser == null)
                {
                    //// Lưu thông tin người dùng vào bảng User
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
                        return RedirectToAction(nameof(NotNulls));
                    }
                    return BadRequest("false");
                }
                var identityUserSignIn = await getRoleAndClaims(checkUser);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identityUserSignIn));
                _httpContext.Session.SetString(key, JsonSerializer.Serialize(checkUser));


                return RedirectToAction(nameof(NotNulls));
            }
            return RedirectToAction(nameof(nulls));

        }
        public async Task<ClaimsIdentity> getRoleAndClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                await _userManager.AddToRoleAsync(user, "client");
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
        [Authorize]
        public IActionResult NotNulls()
        {
            return View();
        }
        public IActionResult nulls()
        {
            return View();
        }

    }
}
