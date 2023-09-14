using ASMC5.Models;
using BILL.ViewModel.Account;
using BILL.ViewModel.Cart;
using BILL.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using TodoList.Models.SeedWork;

namespace ClientUI.Service.IResponsitories
{
    public interface IResponsitoriesCartDetail
    {
        public Task<bool> CreatCartDetail(CartDetailVM p);
        public Task<bool> EditCartDetail(Guid id, CartDetailVM p);
        public Task<bool> EditCartDetailPaied(Guid id);
        public Task<bool> DelCartDetail(Guid id);
        public Task<List<CartDetailVM>> GetAllCartDetail();


        public Task<CartDetailVM> GetCartDetailById(Guid id);

    }
}
