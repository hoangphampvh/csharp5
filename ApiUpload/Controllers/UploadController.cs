using ASMC5.data;
using BILL.Serviece.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUploadFileService _uploadFile;

        public FileController(IWebHostEnvironment env,IUploadFileService uploadFile)
        {
            _uploadFile = uploadFile;
            _env = env;
         
        }

        [HttpPost("UploadFile")]
        public async Task<ActionResult<List<UploadResult>>> UploadFile(List<IFormFile> files)
        {
            var result =await _uploadFile.UploadFile(files);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}