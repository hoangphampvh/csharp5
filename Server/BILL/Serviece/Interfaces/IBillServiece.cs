using ASMC5.Models;
using BILL.ViewModel.Bill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    public interface IBillServiece
    {
        public Task<bool> CreatBill(BillVM p);
        public Task<bool> EditBill(Guid id, BillVM p);
        public Task<bool> DelBill(Guid id);
        public Task<bool> confirmBill(Guid id);
        public Task<List<Bill>> GetAllBill();

        public Task<Bill> GetBillById(Guid id);
    }
}
