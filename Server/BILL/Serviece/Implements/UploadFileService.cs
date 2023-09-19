using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Product;
using BILL.ViewModel.UploadFile;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModel.Role;
using XAct;

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
        public async Task<bool> UploadFile(IFormFile file, CancellationToken cancellationtoken)
        {
            var result = await WriteFile(file);
            if (result.IsNullOrEmpty())
                return false;
            return true;
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            try
            {

                filename = file.FileName;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
            }
            return filename;
        }
        public async Task<List<ProductVM>> UploadFileExcel(IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                var result = await WriteFile(file);
                if (result.IsNullOrEmpty())
                {
                    return new List<ProductVM>();
                }

                var lstPro = new List<ProductVM>();
                var excelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", result);

                using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // Define an array of expected column headers
                    string[] expectedHeaders = { "STT", "Name", "Price", "Quantity" };

                    // Check if the worksheet has the expected column headers
                    if (worksheet.Dimension == null || worksheet.Dimension.End.Row < 1 || worksheet.Dimension.End.Column < expectedHeaders.Length)
                    {
                        return new List<ProductVM>();
                    }

                    for (int i = 1; i <= expectedHeaders.Length; i++)
                    {
                        if (!worksheet.Cells[1, i].Text.Equals(expectedHeaders[i - 1], StringComparison.OrdinalIgnoreCase))
                        {
                            return new List<ProductVM>();
                        }
                    }

                    int rowCount = worksheet.Dimension.End.Row;

                    for (int i = 2; i <= rowCount; i++) // Start from row 2 to skip the header row
                    {
                        var row = worksheet.Cells[i, 1, i, worksheet.Dimension.End.Column];
                        var values = row.Select(cell => cell.Text).ToList();

                        var product = new ProductVM
                        {
                            STT = values.Count > 0 ? int.Parse(values[0]) : 0,
                            Name = values.Count > 1 ? values[1] : null,
                            Price = values.Count > 2 ? decimal.Parse(values[2]) : 0,
                            Quantity = values.Count > 3 ? int.Parse(values[3]) : 0,
                            Status = values.Count > 4 ? int.Parse(values[4]) : 0,
                            Supplier = values.Count > 5 ? values[5] : null,
                            Description = values.Count > 6 ? values[6] : null,
                            UrlImage = values.Count > 7 ? values[7] : null,
                            CreateDate = values.Count > 8 ? DateTime.Parse(values[8]) : DateTime.MinValue,
                            CreateBy = values.Count > 9 ? Guid.Parse(values[9]) : Guid.Empty
                        };

                        lstPro.Add(product);
                    }
                }

                return lstPro;
            }
            catch (Exception ex)
            {
                // Handle or log the error here
                throw ex;
            }
        }

    }
}
