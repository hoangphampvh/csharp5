using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Bill;
using BILL.ViewModel.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Implements
{
    public class BillServiece : IBillServiece
    {
        ASMDBContext context;
        private readonly IBillDetailServiece _billDetailServiece;
        private readonly IProductServiece _productServiece;
        public BillServiece(IBillDetailServiece billDetailServiece, IProductServiece productServiece)
        {
            _billDetailServiece = billDetailServiece;
            _productServiece = productServiece;
            this.context = new ASMDBContext();
        }

        public async Task<bool> confirmBill(Guid id)
        {
            try
            {
                var list = await context.Bills.ToListAsync();
                Bill obj = list.FirstOrDefault(c => c.ID == id);
                var detailBillList = await _billDetailServiece.GetAllBillDetail();
                var BillDetail = detailBillList.FirstOrDefault(p=>p.BillID == obj.ID);
                var IdProductInBillDetail = BillDetail.ProductID;
                var product = await context.Products.FirstOrDefaultAsync(p => p.ID == IdProductInBillDetail);
                if (product != null)
                {
                    foreach (var item in detailBillList.Where(p=>p.ProductID ==IdProductInBillDetail))
                    {
                        product.Quantity = product.Quantity - item.Quantity;
                        if (product.Quantity == 0) product.Status = 1;
                        product.Status = 0;
                        var UpdateProductVM = new ProductUpdateConfirmVM();
                        UpdateProductVM.Status = product.Status;
                        UpdateProductVM.Quantity = product.Quantity;
                        
                        if(await _productServiece.UpdateProductWhenConfirmBill(IdProductInBillDetail, UpdateProductVM))
                        {
                            obj.Status = 2; // confirm thanh cong
                            context.Bills.Update(obj);
                            context.SaveChanges();
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> CreatBill(BillVM p)
        {
            try
            {
                var user = await context.Users.FindAsync(p.UserID);

                if (user == null)
                {
                    return false;
                }
                var bill = new Bill() {
                    ID =  p.Id,
                    DateCreatBill = p.DateCreatBill,
                    DateOfPayment = p.DateOfPayment,
                    Status = 0,
                    UserID = user.Id,
                    User = user,
                };                
                context.Add(bill);
                context.SaveChanges();
                return true;
            
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task< bool> DelBill(Guid id)
        {
            try
            {
                var list = context.Bills.ToList();
                var obj = list.FirstOrDefault(c => c.ID == id);
                context.Bills.Remove(obj);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task <bool> EditBill(Guid id, BillVM p)
        {
            try
            {
                var listobj = context.Bills.ToList();
                var obj = listobj.FirstOrDefault(c => c.ID == id);
                obj.DateCreatBill = p.DateCreatBill;
                obj.DateOfPayment = p.DateOfPayment;
                obj.Status = p.Status;
                context.Bills.Update(obj);
                context.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task< List<Bill>> GetAllBill()
        {
                return context.Bills.ToList();
        }

        public async Task< Bill> GetBillById(Guid id)
        {
            var list = context.Bills.AsQueryable().ToList();
            return list.FirstOrDefault(c => c.ID == id);
        }
    }
}
