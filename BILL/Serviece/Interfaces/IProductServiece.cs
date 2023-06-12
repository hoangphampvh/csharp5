using ASMC5.Models;
using BILL.ViewModel.Product;
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
        public Task<bool> DelProduct(Guid id);
        public Task<List<Product>> GetAllProduct();
        public Task<List<Product>> GetAllProductActive();
        public Task<ProductVM> GetProductById(Guid id);
        public Task<bool> UpdateProductWhenConfirmBill(Guid id, ProductUpdateConfirmVM p);
    }
}
