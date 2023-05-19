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
    internal class CartDetailServiece : ICartDetailServiece
    {
        ASMDBContext context;
        public CartDetailServiece()
        {
            this.context = new ASMDBContext();
        }
        public bool CreatCartDetail(CartDetail p)
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

        public bool DelCartDetail(Guid id)
        {
            try
            {
                var list = context.cartDetails.ToList();
                var obj = list.FirstOrDefault(c => c.ID == id);
                context.cartDetails.Remove(obj);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool EditCartDetail(Guid id, CartDetail p)
        {
            try
            {
                var listobj = context.cartDetails.ToList();
                var obj = listobj.FirstOrDefault(c => c.ID == id);
                obj.Quantity = p.Quantity;               
                context.cartDetails.Update(obj);
                context.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<CartDetail> GetAllCartDetail()
        {
            return context.cartDetails.ToList();
        }

        public CartDetail GetCartDetailById(Guid id)
        {
            var list = context.cartDetails.AsQueryable().ToList();
            return list.FirstOrDefault(c => c.ID == id);
        }
    }
}
