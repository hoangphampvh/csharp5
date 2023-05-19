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
    internal class ProductServiece : IProductServiece
    {
        ASMDBContext context;
        public ProductServiece()
        {
            this.context = new ASMDBContext();
        }
        public bool CreatProduct(Product p)
        {
            try
            {
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
