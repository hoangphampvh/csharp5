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
    internal class BillDetailServiece : IBillDetailServiece
    {
        ASMDBContext context;
        public BillDetailServiece()
        {
            this.context = new ASMDBContext();
        }

        public bool CreatBillDetail(BillDetail p)
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

        public bool DelBillDetail(Guid id)
        {
            try
            {
                var list = context.billDetails.ToList();
                var obj = list.FirstOrDefault(c => c.ID == id);
                context.billDetails.Remove(obj);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool EditBillDetail(Guid id, BillDetail p)
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

        public List<BillDetail> GetAllBillDetail()
        {
            return context.billDetails.ToList();
        }

        public BillDetail GetBillDetailById(Guid id)
        {
            var list = context.billDetails.AsQueryable().ToList();
            return list.FirstOrDefault(c => c.ID == id);
        }
    }
}
