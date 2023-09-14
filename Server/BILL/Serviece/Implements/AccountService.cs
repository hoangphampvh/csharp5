using ASMC5.data;
using ASMC5.Models;
using ASMC5.ViewModel;
using BILL.CacheRedisData;
using BILL.Serviece.Interfaces;
using BILL.ViewModel;
using BILL.ViewModel.Account;
using BILL.ViewModel.Cart;
using BILL.ViewModel.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Token = ASMC5.Models.Token;

namespace BILL.Serviece.Implements
{
    public class AccountService : IAccountService
    {
        private readonly ASMDBContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ICartServiece _cartServiece;
        protected CacheData cacheData = CacheData.Instance;
        private static readonly string key = "HOANG_PHAM_NGHIA_HUNG_NAM_DINH18";
        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, ICartServiece cartServiece,IConfiguration configuration)
        {
            _context = new ASMDBContext();
            _userManager = userManager;
            _signInManager = signInManager;
            _cartServiece = cartServiece;
            _configuration = configuration; 

        }

        public async Task<LoginResponesVM> Validate(LoginRequestVM loginRequest)
        {
            //cấp token
            var token = await GenerateToken(loginRequest);
            return new LoginResponesVM { Successful = true, Mess = "Successful",Data =  token };
        }

        public async Task<TokenModel> GenerateToken(LoginRequestVM loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.UserName);
            if (user == null || await _userManager.CheckPasswordAsync(user, loginRequest.Password) == false)
            {
                return new TokenModel();
            }
            var secretKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var rolesOfUser = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,loginRequest.UserName),
                new Claim("Id",user.Id.ToString()),
                new Claim("userName",user.UserName.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             

            };
            foreach (var role in rolesOfUser)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var ClaimIdentity = new ClaimsIdentity(claims);

          

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(ClaimIdentity),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(secretKeyBytes, SecurityAlgorithms.HmacSha256),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                IssuedAt = DateTime.UtcNow
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();
            var check =await _context.tokens.FirstOrDefaultAsync(p => p.IdUser == user.Id);
            if(check != null)
            {
                refreshToken = check.RefreshToken;
            }
            else
            {
                //Lưu database
                var refreshTokenEntity = new Token
                {
                    Id = Guid.NewGuid(),

                    IdUser = user.Id,
                    RefreshToken = refreshToken,
                    IsUsed = false,
                    IsRevoked = false,
                    Iaced = DateTime.UtcNow,
                    Expired = DateTime.UtcNow.AddHours(1),

                };

                await _context.AddAsync(refreshTokenEntity);
                await _context.SaveChangesAsync();
            }

            // lưu thông tin người dùng vào redis server
            user.AccessToken = accessToken;
            await cacheData.SetObj(keyUserQuanTri(user.Id.ToString(), user.UserName), user, 30);

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }
        public static string keyUserQuanTri(string Id, string userName)
        {
            object keyRedis = new
            {
                UserName = userName,
                Id = Id,
            };

            return CacheData.Instance.GenerateKey(keyRedis);
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        private static DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
        public static async Task<bool> checkAccessToken(string AccessToken)
        {


            var jwtTokenHandler = new JwtSecurityTokenHandler();
            CacheData cacheData = CacheData.Instance;
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(key);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false
            };
            //task 1: accesstoken valid format
            var tokenInVerification = jwtTokenHandler.ValidateToken(AccessToken, tokenValidateParam, out var validatedToken);

            //task 2: check alg
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                if (!result)//false
                {
                    return false;

                }
            }

            //task 3: check accessToken expire
            var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
            if (expireDate > DateTime.UtcNow)
            {
                return false;
            }
            var simplePrinciple = tokenInVerification;
            var identity = simplePrinciple?.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            IEnumerable<Claim> claims = identity.Claims;
            var username = claims.Where(p => p.Type == "userName").FirstOrDefault()?.Value;
            var Id = claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value;


            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(Id))
                return false;

            // check keyUser
            if (!await cacheData.IsKeyExists(keyUserQuanTri(Id, username))) return false;
            return true;
        }
       
        public async Task<LoginResponesVM> RenewToken(TokenModel tokenDTO)
        {
           
            try
            {
               
                if (await checkAccessToken(tokenDTO.AccessToken))
                {
                    // task 4: check refresktoken exist in DB
                    Token storedToken = _context.tokens.FirstOrDefault(x => x.RefreshToken == tokenDTO.RefreshToken);
                    if (storedToken != null)
                    {
                        // task 5 check refresh token isuser/revoked  
                        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == storedToken.IdUser);
                        if (user != null)
                        {
                            LoginRequestVM loginRequestVM = new LoginRequestVM();
                            loginRequestVM.UserName = user.UserName;
                            loginRequestVM.Password = user.Password;
                            if (!storedToken.IsUsed && user.Id == storedToken.IdUser && !storedToken.IsRevoked && !storedToken.IsActive && storedToken.Expired >= DateTime.UtcNow || storedToken.Expired >= DateTime.UtcNow)
                            {
                                // update
                                storedToken.IsRevoked = true;
                                storedToken.IsUsed = true;
                                storedToken.IsActive = true;
                                _context.Update(storedToken);
                                await _context.SaveChangesAsync();
                                var tokenValidate = await GenerateToken(loginRequestVM);

                                return new LoginResponesVM { Successful = true, Mess = "Successful", Data = tokenValidate };
                            }
                        }

                    }
                    return new LoginResponesVM { Successful = false, Mess = "Token is not in DB" };

                }
                return new LoginResponesVM { Successful = false, Mess = "Token is worrong" };
            }
            catch
            {
                return new LoginResponesVM { Successful = false, Mess = "Some Thing Error" };
            }
        }

        public async Task<bool> SignUp(SignUpVM p)
        {
            try
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = p.UserName,
                    PhoneNumber = p.PhoneNumber,
                    Dateofbirth = p.Dateofbirth,
                    Status = 0,   // quy uoc 0 có nghĩa là đang hđ
                    DiaChi = p.DiaChi,
                    Email = p.Email,
                    Password = p.Password,

                };
                var result = await _userManager.CreateAsync(user, p.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Client");
                    var cartVM = new CartVN();
                    cartVM.UserId = user.Id;
                    cartVM.Description = "Giỏ Hàng";
                    if (await _cartServiece.CreatCart(cartVM))
                    {
                        Console.WriteLine("cart create thanh cong");
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}



