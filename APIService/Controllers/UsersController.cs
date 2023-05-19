using ASMC5.Models;
using ASMC5.ViewModel;
using BILL.Serviece.Interfaces;
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
            if(user !=null) return Ok(user);
            return BadRequest();
        }
        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var user = await _userServiece.GetAllUserActive();
            if (user != null) return Ok(user);
            return BadRequest();
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserAsync(Guid Id)
        {          
            var result = await _userServiece.DelUser(Id);
            return Ok(result);
            
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateUserAsync([FromForm]SignUpVM user)
        {
            var result = await _userServiece.CreatUser(user);
            return Ok(result);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody]User user)
        {
            var result = await _userServiece.EditUser(id,user);
            return Ok(result);
        }
    }
}
