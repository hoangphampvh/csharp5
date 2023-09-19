using ASMC5.Models;
using BILL.ViewModel.Product;
using BLL.ViewModel.ModelConfiguration.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    public interface IProductServiece
    {
        public Task<bool> CreatProduct(ProductVM p);
        public Task <bool> EditProduct(Guid id, ProductVM p);
        public Task<PagedList<ProductVM>> GetAllProductActive(ProductListSearch productListSearch);
        public Task<List<ProductVM>> GetAllProduct();
        public Task<ProductVM> GetProductById(Guid id);
        public Task<bool> UpdateProductWhenConfirmBill(Guid id, ProductUpdateConfirmVM p);
        public Task<bool> DelProduct(Guid id);

    }
}
