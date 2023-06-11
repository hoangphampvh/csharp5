using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Bill;
using Microsoft.EntityFrameworkCore;
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

                var billdetail = new BillDetail()
                {
                    ID = Guid.NewGuid(),
                    CodeBill = p.CodeBill,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Status = 0,
                    BillID = p.BillID,
                    ProductID = p.ProductID,
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
        public async Task<bool> CreateBillDetailStatus0(Guid id)
        {
            try
            {
                var listobj =await context.billDetails.ToListAsync();
                var obj = listobj.FirstOrDefault(c => c.ID == id);
                obj.Status = 0;
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
