using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModel.Role;

namespace BILL.Serviece.Implements
{
    public class UploadFileService : IUploadFileService
    {
        private readonly ASMDBContext _context;
        private readonly IWebHostEnvironment _env;
        public UploadFileService(IWebHostEnvironment env)
        {      
            _context = new ASMDBContext();
            _env = env;
        }
        public async Task<bool> UploadFile(List<IFormFile> files)
        {
            List<UploadResult> uploadResults = new List<UploadResult>();

            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                //var untrustedFileName = file.FileName;
                var uploadDirectory = Path.Combine(_env.ContentRootPath, "uploads");
                uploadResult.FileName = file.FileName;
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory); // Tạo thư mục nếu chưa tồn tại.
                }
                //var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);
                var path = Path.Combine(uploadDirectory, file.Name);

                await using FileStream fs = new(path, FileMode.Create);
                await file.CopyToAsync(fs);

              
                uploadResult.ContentType = file.ContentType;
                uploadResults.Add(uploadResult);

                _context.Uploads.Add(uploadResult);
            }

            await _context.SaveChangesAsync();
            return true;
            
        }
    }
}
