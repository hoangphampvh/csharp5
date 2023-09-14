using ASMC5.Models;
using BILL.Serviece.Implements;
using BILL.Serviece.Interfaces;
using BILL.ViewModel;
using BILL.ViewModel.Account;
using BILL.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _userServiece;

        public AccountController(IAccountService userServiece)
        {
            _userServiece = userServiece;

        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestVM loginRequest)
        {
            var result = await _userServiece.Validate(loginRequest);
            if(result == null)
            {
                return BadRequest("khong co access");
            }
            return Ok(result);
        }
        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel tokenDTO)
        {
            var result = await _userServiece.RenewToken(tokenDTO);
            return Ok(result);
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpVM signUp)
        {
            var result = await _userServiece.SignUp(signUp);
            return Ok(result);
        }
        

    }
}
