using ASMC5.Models;
using BILL.Serviece.Implements;
using BILL.Serviece.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ViewModel.ViewModel.Role;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private readonly IUploadFileService _positionServiece;
        public UploadFileController(IUploadFileService positionServiece)
        {
            _positionServiece = positionServiece;
        }
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            var result = await _positionServiece.UploadFile(files);
            if(result) return Ok(result);
            return BadRequest();
        }
       
    }
}
