using ASMC5.Models;
using BILL.ViewModel.Cart;
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
        public Task<bool> UploadFile(List<IFormFile> files);

    }
}
