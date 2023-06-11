using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Bill;
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
        private readonly IBillServiece _billServiece;
        private readonly IBillDetailServiece _billDetailServiece;
        private readonly IProductServiece _productServiece;
        public CartDetailServiece(IBillDetailServiece billDetail, IBillServiece billServiece, IProductServiece productServiece)
        {
            _billDetailServiece = billDetail;
            _billServiece = billServiece;
            this.context = new ASMDBContext();
            _productServiece = productServiece;
        }
        public async Task <bool> CreatCartDetail(CartDetailVM p)
        {
            try
            {            
                var Cartdetail = new CartDetail()
                {
                    ID = Guid.NewGuid(),
                    Quantity = p.Quantity,
                    Status = 0,
                    ProductID = p.ProductID,
                    UserID = p.UserID,         
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
                obj.Status = p.Status;
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
                obj.Status = 0; // 
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
        // thanh toan
        public async Task<bool> Pay(CartDetailVM cartDetail)
        {
            try
            {
                var cartList = await GetAllCartDetail();
                var cartListNow = cartList.Where(p => p.Status == 0);

                var CartListVM = new List<CartDetailVM>();
                foreach (var item in cartListNow)
                {
                    var CartDetail = new CartDetailVM();
                    cartDetail.Status = item.Status;
                    cartDetail.Quantity = item.Quantity;
                    cartDetail.UserID = item.UserID;
                    cartDetail.ProductID = item.ProductID;

                    CartListVM.Add(cartDetail);
                }
                // tao bill 
                BillVM bill = new BillVM();
                bill.Status = 2;
                bill.DateCreatBill = DateTime.Now;
                bill.UserID = cartDetail.UserID;
                bill.Id = Guid.NewGuid();
                if (await _billServiece.CreatBill(bill))
                {
                    foreach (var item in CartListVM)
                    {
                        // tao bill detail
                        BillDetailVM billDetail = new BillDetailVM();
                        billDetail.BillID = bill.Id;
                        billDetail.ProductID = item.ProductID;
                        billDetail.Quantity = item.Quantity;
                        billDetail.CodeBill = (cartListNow.Count() + "MaHoaDon" + 1).ToString();
                        billDetail.Status = 0;
                        var product =await _productServiece.GetProductById(item.ProductID);
                        if (product != null)
                        {
                            billDetail.Price = product.Price;
                        }
                        if (await _billDetailServiece.CreatBillDetail(billDetail))
                        {
                            Console.WriteLine("billDetail Add True");
                        }
                    }
                }
                foreach (var item in cartListNow)
                {
                    item.Status = 2;
                    if(await EditCartDetailPaied(item.ID))
                    {
                        Console.WriteLine("EditCartDetailPaied");
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
            
        }
    }
}
