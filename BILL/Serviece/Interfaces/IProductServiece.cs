using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    public interface IProductServiece
    {
        public Task<bool> CreatProduct(Product p);
        public Task <bool> EditProduct(Guid id, Product p);
        public Task<bool> DelProduct(Guid id);
        public Task<List<Product>> GetAllProduct();
        public Task<List<Product>> GetAllProductActive();
        public Task<Product> GetProductById(Guid id);
    }
}
