using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    internal interface IBillServiece
    {
        public Task<bool> CreatBill(Bill p);
        public Task<bool> EditBill(Guid id, Bill p);
        public Task<bool> DelBill(Guid id);
        public Task<List<Bill>> GetAllBill();

        public Task<Bill> GetBillById(Guid id);
    }
}
