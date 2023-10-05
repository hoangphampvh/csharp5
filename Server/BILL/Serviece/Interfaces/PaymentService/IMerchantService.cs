using ASMC5.Models;
using BILL.ViewModel.Bill;
using BILL.ViewModel.Product;
using BLL.ViewModel.ModelConfiguration.SeedWork;
using BLL.ViewModel.PaymentConfiguration.MerchantVM;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Serviece.Interfaces
{
    public interface IMerchantService
    {
        public Task<bool> CreateMerchant(MerchantCreateVM p);
        public Task<bool> EditMerchant(Guid id, MerchantUpdateVM p);
        public Task<bool> DelMerchant(Guid id);
        public Task<PagedList<MerchantVM>> GetAllMerchant(ProductListSearch productListSearch);
        public Task<MerchantVM> GetMerchantById(Guid id);
        public Task<SetActiveMerchantVM> SetActive(string id, SetActiveMerchantVM request);
    }
}
