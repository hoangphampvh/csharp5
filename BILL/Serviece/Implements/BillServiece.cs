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
    internal class BillServiece : IBillServiece
    {
        ASMDBContext context;
        public BillServiece()
        {
            this.context = new ASMDBContext();
        }
        public async Task<bool> CreatBill(Bill p)
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

        public async Task <bool> EditBill(Guid id, Bill p)
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
