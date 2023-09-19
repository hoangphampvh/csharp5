using BILL.ViewModel.Account;
using BILL.ViewModel.Product;
using BLL.ViewModel.ModelConfiguration.SeedWork;

namespace ClientUI.Service.IResponsitories
{
    public interface IResponsitoriesProduct
    {
        public Task<PagedList<ProductVM>> GetAllProductActive(ProductListSearch ProductListSearch);
        public Task<bool> CreateNewProduct(ProductVM product);
        public Task<ProductVM> GetById(Guid Id);
        public Task<bool> UpdateNewProduct(ProductVM product, Guid Id);
        public Task<bool> DeleteById(Guid Id);
        public Task<List<ProductVM>> GetAllProduct();
    }
}
