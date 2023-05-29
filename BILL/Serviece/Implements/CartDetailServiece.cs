using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Implements
{
    public class CartDetailServiece : ICartDetailServiece
    {
        ASMDBContext context;
        public CartDetailServiece()
        {
            this.context = new ASMDBContext();
        }
        public async Task <bool> CreatCartDetail(CartDetailVM p)
        {
            try
            {
                var carts = await context.carts.FindAsync(p.CartID);
                var product = await context.Products.FindAsync(p.ProductID);

               
                var Cartdetail = new CartDetail()
                {
                    ID = new Guid(),
                    Quantity = p.Quantity,
                    Status = 0,
                    ProductID = product.ID,
                    CartID = carts.UserId,
                    Product = product,
                    Cart = carts,
                    
                   
                              
                };
                context.AddAsync(Cartdetail);
                context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task< bool> DelCartDetail(Guid id)
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

        public async Task< bool> EditCartDetail(Guid id, CartDetailVM p)
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

        public async Task< List<CartDetail>> GetAllCartDetail()
        {
            return context.cartDetails.ToList();
        }

        public async Task< CartDetail> GetCartDetailById(Guid id)
        {
            var list = context.cartDetails.AsQueryable().ToList();
            return list.FirstOrDefault(c => c.ID == id);
        }
    }
}
