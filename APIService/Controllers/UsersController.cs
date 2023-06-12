using ASMC5.Models;
using BILL.Serviece.Implements;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Account;
using BILL.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServiece _userServiece;

        public UsersController(IUserServiece userServiece)
        {
            _userServiece = userServiece;

        }
        [HttpGet("GetAll")]

        public async Task<IActionResult> GetAllAsync()
        {
            var user = await _userServiece.GetAllUser();
            if (user != null) return Ok(user);
            return BadRequest();
        }
        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var user = await _userServiece.GetAllUserActive();
            if (user != null) return Ok(user);
            return BadRequest();
        }
        [HttpGet("GetById/{id}")]

        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userServiece.GetUserById(id);
            if (user != null) return Ok(user);
            return BadRequest();
        }

        [HttpGet("GetByName/{name}")]

        public async Task<IActionResult> GetUserByName(string name)
        {
            var user = await _userServiece.GetUserByName(name);
            if (user != null) return Ok(user);
            return BadRequest();
        }

        [HttpDelete("Delete/{Id}")]


        public async Task<IActionResult> DeleteUserAsync(Guid Id)
        {
            var result = await _userServiece.DelUser(Id);
            return Ok(result);

        }

        [HttpDelete("DeleteListUserById")]


        public async Task<IActionResult> DeleteUserAsync(List<Guid> Ids)
        {
            var result = await _userServiece.DelUsers(Ids);
            return Ok(result);

        }

        [HttpPost("Create")]


        public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateVM user)
        {
            var result = await _userServiece.CreatUser(user);
            return Ok(result);
        }
        [HttpPut("Update/{id}")]


        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UserEditVM user)
        {
            var result = await _userServiece.EditUser(id, user);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginWithJWT(LoginRequestVM loginRequest)
        {
            var result = await _userServiece.LoginWithJWT(loginRequest);
            return Ok(result.Token);
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpVM signUp)
        {
            var result = await _userServiece.SignUp(signUp);
            return Ok(result);
        }


    }
}
