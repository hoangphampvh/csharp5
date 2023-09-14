using ASMC5.Models;
using BILL.ViewModel.Account;
using BILL.ViewModel.Cart;
using BILL.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using TodoList.Models.SeedWork;

namespace ClientUI.Service.IResponsitories
{
    public interface IResponsitoriesUpload
    {

        public Task<bool> UploadFiles(List<Stream> files);
        public List<ProductVM> GetListProductFromExcel(string fileName);
    }
}
