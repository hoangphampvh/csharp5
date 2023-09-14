using BILL.CacheRedisData;
using BILL.Serviece.Implements;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc.Filters;

using XAct.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DemoRedis.Attributes
{

    //IAsyncActionFilter 
    //nó là một phần của middleware pipeline và được sử dụng để thực hiện các hành động trước và sau khi một Action được thực thi.

    public class AuthorizeUserAttribute : Attribute, IAsyncActionFilter
    {    
        protected IServer server { get; set; }
        public AuthorizeUserAttribute() { }
        //OnActionExecutionAsync để xử lý các yêu cầu HTTP trước và sau khi nó được xử lý bởi một Action.
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader != null)
            {
                var token = authorizationHeader.Replace("Bearer ", string.Empty);
                if (!token.Equals("Bearer") && ! await AccountService.checkAccessToken(token))
                {
                   return;
                }
                await next();
            }
            await next();
        }
       
    }
}
