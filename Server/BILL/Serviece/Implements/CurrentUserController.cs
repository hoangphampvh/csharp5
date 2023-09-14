using ASMC5.ViewModel;
using BILL.CacheRedisData;
using BILL.Serviece.Implements;
using BILL.Serviece.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<UserVM> GetCurrentUserInfo()
    {
        var service = CacheData.Instance;
        var secretKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
        var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = secretKeyBytes,
            ValidateIssuer = false,
            ValidateAudience = false
        };
        if (authorizationHeader != null)
        {
            var token = authorizationHeader.Replace("Bearer ", string.Empty);
            if (await AccountService.checkAccessToken(token))
            {
                ClaimsPrincipal principal;
                try
                {
                    principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                }
                catch (SecurityTokenException)
                {
                    throw;
                }
                string key = AccountService.keyUserQuanTri(principal.FindFirstValue("Id"), principal.FindFirstValue(ClaimTypes.Name));
                if (await service.IsKeyExists(key))
                {
                    return await service.GetObj<UserVM>(key);
                }
            }
        }

        return null;
    }
    public UserVM GetCurrentUserInfos()
    {
        var service = CacheData.Instance;
        var secretKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
       
        var jwtToken = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = secretKeyBytes,
            ValidateIssuer = false,
            ValidateAudience = false
        };
        if (jwtToken != null )
        {
          

            ClaimsPrincipal principal;
            try
            {
                
                
                    principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out _);
                
            }
            catch (SecurityTokenException)
            {
                throw;
            }
            string key = AccountService.keyUserQuanTri(principal.FindFirstValue("Id"), principal.FindFirstValue(ClaimTypes.Name));
            return service.GetObjs<UserVM>(key);


        }

        return null;
    }
}
