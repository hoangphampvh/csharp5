using ASMC5.Models;
using BILL.ViewModel.Cart;
using BILL.ViewModel.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    public interface IUploadFileService
    {
        public Task<bool> UploadFile(IFormFile file, CancellationToken cancellationtoken);
        public Task<List<ProductVM>> UploadFileExcel(IFormFile file, CancellationToken cancellationToken);

    }
}
