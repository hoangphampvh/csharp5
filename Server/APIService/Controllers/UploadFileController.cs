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
        public async Task<IActionResult> UploadFile(IFormFile files,CancellationToken token)
        {
            var result = await _positionServiece.UploadFile(files,token);
            if(result) return Ok(result);
            return BadRequest();
        }
        [HttpPost("Excel")]
        public async Task<IActionResult> UploadFileExcel(IFormFile files, CancellationToken token)
        {
            var result = await _positionServiece.UploadFileExcel(files, token);
            if (result!=null&& result.Count() >0) return Ok(result);
            return BadRequest();
        }

    }
}
