using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASMC5.Models;

namespace BILL.Serviece.Interfaces
{
    internal interface IBillDetailServiece
    {
        public  Task<bool> CreatBillDetail(BillDetail p);
        public Task<bool> EditBillDetail(Guid id, BillDetail p);
        public Task<bool> DelBillDetail(Guid id);
        public Task<List<BillDetail>> GetAllBillDetail();

        public Task<BillDetail> GetBillDetailById(Guid id);
    }
}
