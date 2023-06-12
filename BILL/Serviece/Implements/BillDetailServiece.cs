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
               await context.AddAsync(billdetail);
               await context.SaveChangesAsync();
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
        public async Task<List<BillDetailVM>> GetAllBillDetail()
        {
            var listBillDetail = await context.billDetails.ToListAsync();
            var listBillDetailVM = new List<BillDetailVM>();
            foreach (var item in listBillDetail)
            {
                var billDetailVM = new BillDetailVM();
                billDetailVM.Price = item.Price;
                billDetailVM.Quantity = item.Quantity;
                billDetailVM.CodeBill = item.CodeBill;
                billDetailVM.Status = item.Status;
                billDetailVM.BillID = item.BillID;
                billDetailVM.ProductID = item.ProductID;
                listBillDetailVM.Add(billDetailVM);
            }
            return listBillDetailVM;
        }

        public async Task<BillDetail> GetBillDetailById(Guid id)
        {
            var list = context.billDetails.AsQueryable().ToList();
            return list.FirstOrDefault(c => c.ID == id);
        }
        public async Task<List<BillDetailView>> ListBillDetail()
        {
            var billDetailViews = await context.billDetails
            .Include(bd => bd.Product)
        .Include(bd => bd.Bill)
            .ThenInclude(b => b.User)
        .Select(bd => new BillDetailView
        {
            ID = bd.ID,
            CodeBill = bd.CodeBill,
            Price = bd.Price,
            Quantity = bd.Quantity,
            Status = bd.Status,
            BillID = bd.BillID,
            ProductID = bd.ProductID,
            NameProduct = bd.Product.Name,
            UserName = bd.Bill.User.UserName
        })
        .ToListAsync();
            return billDetailViews;
        }

    }
}
