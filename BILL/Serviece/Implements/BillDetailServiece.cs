using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Bill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Implements
{
    public class BillDetailServiece : IBillDetailServiece
    {
        ASMDBContext context;
        public BillDetailServiece()
        {
            this.context = new ASMDBContext();
        }

        public async Task<bool> CreatBillDetail(BillDetailVM p)
        {
            try
            {
                var bill = await context.Bills.FindAsync(p.BillID);
                var product = await context.Products.FindAsync(p.ProductID);

                if (bill == null)
                {
                    return false;
                }
                if (product == null)
                {
                    return false;
                }
                var billdetail = new BillDetail()
                {
                    ID = new Guid(),
                    CodeBill = p.CodeBill,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Status = 0,
                    BillID = bill.ID,
                    ProductID = product.ID,
                    Product = product,
                    Bill = bill
                };
                context.Add(billdetail);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> DelBillDetail(Guid id)
        {
            try
            {
                var list = context.billDetails.ToList();
                var obj = list.SingleOrDefault(c => c.ID == id);
                context.billDetails.Remove(obj);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> EditBillDetail(Guid id, BillDetailVM p)
        {
            try
            {
                var listobj = context.billDetails.ToList();
                var obj = listobj.FirstOrDefault(c => c.ID == id);
                obj.CodeBill = p.CodeBill;
                obj.Price = p.Price;
                obj.Quantity = p.Quantity;
                context.billDetails.Update(obj);
                context.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<BillDetail>> GetAllBillDetail()
        {
            return context.billDetails.ToList();
        }

        public async Task<BillDetail> GetBillDetailById(Guid id)
        {
            var list = context.billDetails.AsQueryable().ToList();
            return list.FirstOrDefault(c => c.ID == id);
        }
    }
}
