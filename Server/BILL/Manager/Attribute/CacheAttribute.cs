using BILL.CacheRedisData;
using BILL.Serviece.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoRedis.Attributes
{

    //IAsyncActionFilter 
    //nó là một phần của middleware pipeline và được sử dụng để thực hiện các hành động trước và sau khi một Action được thực thi.

    public class CacheAttribute : Attribute, IAsyncActionFilter
    {

        public CacheAttribute()
        {
        }


        //OnActionExecutionAsync để xử lý các yêu cầu HTTP trước và sau khi nó được xử lý bởi một Action.
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var service = CacheData.Instance;
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var secretKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HOANG_PHAM_NGHIA_HUNG_NAM_DINH18"));
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
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
                if (token!=null && !token.Equals("Bearer") && await AccountService.checkAccessToken(token))
                {
                    ClaimsPrincipal principal;
                    try
                    {
                        principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                        var cacheResponse = await service.GetString(cacheKey);
                        if (!string.IsNullOrEmpty(cacheResponse))
                        {
                            var contentaResult = new ContentResult { Content = cacheResponse, ContentType = "application/json", StatusCode = 200 };
                            context.Result = contentaResult;
                            return;
                        }
                        var excutedContext = await next();
                        if (excutedContext.Result is OkObjectResult objectResult)
                        {
                            cacheKey = cacheKey + principal.FindFirstValue(ClaimTypes.Name);
                            var result = JsonConvert.SerializeObject(objectResult.Value);
                            await service.SetString(cacheKey, result);
                        }
                    }
                    catch (SecurityTokenException)
                    {
                        throw;
                    }
                }
                await next();
            };
        }
        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key,value) in request.Query.OrderBy(x=>x.Key))
            {
                keyBuilder.Append($"{key} : {value}");
            }
            return keyBuilder.ToString();
        }
    }
}
