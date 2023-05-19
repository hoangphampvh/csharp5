using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    internal interface IProductServiece
    {
        public bool CreatProduct(Product p);
        public bool EditProduct(Guid id, Product p);
        public bool DelProduct(Guid id);
        public List<Product> GetAllProduct();

        public Product GetProductById(Guid id);
    }
}
