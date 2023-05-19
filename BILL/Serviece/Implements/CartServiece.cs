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
    internal class CartServiece : ICartServiece
    {
        ASMDBContext context;
        public CartServiece()
        {
            this.context = new ASMDBContext();
        }
        public bool CreatCart(Cart p)
        {
            try
            {
                p.UserId = Guid.NewGuid();
                context.Add(p);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DelCart(Guid id)
        {
            try
            {
                var list = context.carts.ToList();
                var obj = list.FirstOrDefault(c => c.UserId == id);
                context.carts.Remove(obj);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool EditCart(Guid id, Cart p)
        {
            try
            {
                var listobj = context.carts.ToList();
                var obj = listobj.FirstOrDefault(c => c.UserId == id);
                obj.Description = p.Description;
                obj.Status = p.Status;
                context.carts.Update(obj);
                context.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<Cart> GetAllCart()
        {
            return context.carts.ToList();
        }

        public Cart GetCartById(Guid id)
        {
            var list = context.carts.AsQueryable().ToList();
            return list.FirstOrDefault(c => c.UserId == id);
        }
    }
}
