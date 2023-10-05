using BILL.CacheRedisData;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ultils.Helpers;

namespace Ultils.Manager.JWT
{
    public class AccessToken
    {
        private static readonly string key = "HOANG_PHAM_NGHIA_HUNG_NAM_DINH18";
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
        public static string keyUserQuanTri(string Id, string userName)
        {
            object keyRedis = new
            {
                UserName = userName,
                Id = Id,
            };

            return HashHelper.GenerateKey(keyRedis);
        }
    }
}
