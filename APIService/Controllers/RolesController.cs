using ASMC5.Models;
using BILL.Serviece.Implements;
using BILL.Serviece.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IPositionServiece _positionServiece;
        public RolesController(IPositionServiece positionServiece)
        {
            _positionServiece = positionServiece;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _positionServiece.GetAllPosition();
            return Ok(result);
        }
        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var result =await _positionServiece.GetAllPositionActive();
            return Ok(result);
        }
        [HttpGet("GetRoleById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _positionServiece.GetPositionById(id);
            return Ok(result);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(Position position)
        {
            var result =await _positionServiece.CreatPosition(position);
            return Ok(result);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UodateAsync(Guid id, int status, string name, string NormalizedName, string ConcurrencyStamp)
        {
            var result =await _positionServiece.EditPosition(id,status,name,NormalizedName,ConcurrencyStamp);
            return Ok(result);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result =await _positionServiece.DelPosition(id);
            return Ok(result);
        }
    }
}
