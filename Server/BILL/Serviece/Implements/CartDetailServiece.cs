using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Bill;
using BILL.ViewModel.Cart;
using Microsoft.EntityFrameworkCore;
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
        ICurrentUserProvider currentUserProvider;
        public CartDetailServiece(ICurrentUserProvider currentUserProvider)
        {
            this.context = new ASMDBContext();
            this.currentUserProvider = currentUserProvider;
        }
        public async Task<bool> CreatCartDetail(CartDetailVM p)
        {
            var currentUser = await currentUserProvider.GetCurrentUserInfo();
            try
            {
                var Cartdetail = new CartDetail()
                {
                    ID = Guid.NewGuid(),
                    Quantity = p.Quantity,
                    Status = 0,
                    ProductID = p.ProductID,
                    UserID = currentUser.Id
                };
                await context.AddAsync(Cartdetail);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> DelCartDetail(Guid id)
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

        public async Task<bool> EditCartDetail(Guid id, CartDetailVM p)
        {
            try
            {
                var listobj = context.cartDetails.ToList();
                var obj = listobj.FirstOrDefault(c => c.ID == id);
                obj.Quantity = p.Quantity;
                obj.Status = 0;
                context.cartDetails.Update(obj);
                context.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<bool> EditCartDetailPaied(Guid id)
        {
            try
            {
                var listobj = context.cartDetails.ToList();
                var obj = listobj.FirstOrDefault(c => c.ID == id);
                obj.Status = 2; // 
                context.cartDetails.Update(obj);
                await context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<List<CartDetailVM>> GetAllCartDetail()
        {
            var currentUser = await currentUserProvider.GetCurrentUserInfo();
            var listProduct = await context.Products.ToListAsync();
            var listCartDeail = await context.cartDetails.AsQueryable().Where(p => p.Status == 0 && p.UserID == currentUser.Id).ToListAsync();

            var listUser = await context.Users.ToListAsync();
            var list = from a in listProduct
                       join b in listCartDeail on a.ID equals b.ProductID                   
                       select new CartDetailVM
                       {
                           ProductName = a.Name,
                           UrlImage = a.UrlImage,
                           ID = b.ID,
                           ProductID = a.ID,
                           Price = a.Price,
                           Quantity = b.Quantity,
                           Status = b.Status,

                       };

            return list.ToList();
        }
        public async Task<CartDetailVM> GetCartDetailById(Guid id)
        {
            var listProduct = await context.Products.ToListAsync();
            var listCartDeail = await context.cartDetails.ToListAsync();
            var listDetailCartPaied = listCartDeail.Where(p => p.Status == 0);
            var listUser = await context.Users.ToListAsync();
            var list = from a in listProduct
                       join b in listCartDeail on a.ID equals b.ProductID
                       join c in listUser on b.UserID equals c.Id                     
                       select new CartDetailVM
                       {
                           ProductName = a.Name,
                           UrlImage = a.UrlImage,
                           UserID = c.Id,
                           ID = b.ID,
                           ProductID = a.ID,
                           Price = a.Price,
                           Quantity = b.Quantity,
                           Status = b.Status,

                       };
            if(list!=null && list.Count() > 0)
            {
                return list.FirstOrDefault(P => P.ID == id);
            }
            return new CartDetailVM();


        }
      
    }
}
