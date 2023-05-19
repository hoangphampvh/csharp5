using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Implements
{
    public class ProductServiece : IProductServiece
    {
        private readonly ASMDBContext _context;
        public ProductServiece(ASMDBContext aSMDBContext)
        {
            _context = aSMDBContext;
        }
        public bool CreatProduct(Product p)
        {
            try
            {
                var product = new Product()
                {
                    ID = Guid.NewGuid(),
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    CreateDate = DateTime.Now,
                    Status = 0, // 0 quy ước là kh đh
                    Supplier = p.Supplier,
                    Quantity = p.Quantity,
                    UrlImage = p.UrlImage,
                    
                };
                p.ID = Guid.NewGuid();
                context.Add(p);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DelProduct(Guid id)
        {
            try
            {
                var list = context.Products.ToList();
                var obj = list.FirstOrDefault(c => c.ID == id);
                context.Products.Remove(obj);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool EditProduct(Guid id, Product p)
        {
            try
            {
                var listobj = context.Products.ToList();
                var obj = listobj.FirstOrDefault(c => c.ID == id);
                obj.Name = p.Name;
                obj.Price = p.Price;
                obj.Quantity = p.Quantity;
                obj.Status = p.Status;
                obj.Supplier = p.Supplier;
                obj.Description = p.Description;
                obj.UrlImage = p.UrlImage;
                context.Products.Update(obj);
                context.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<Product> GetAllProduct()
        {
            return context.Products.ToList();
        }

        public Product GetProductById(Guid id)
        {
            var list = context.Products.AsQueryable().ToList();
            return list.FirstOrDefault(c => c.ID == id);
        }
    }
}
