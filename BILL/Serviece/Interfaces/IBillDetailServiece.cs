using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASMC5.Models;
using BILL.ViewModel.Bill;

namespace BILL.Serviece.Interfaces
{
    public interface IBillDetailServiece
    {
        public  Task<bool> CreatBillDetail(BillDetailVM p);
        public Task<bool> EditBillDetail(Guid id, BillDetailVM p);
        public Task<bool> DelBillDetail(Guid id);
        public Task<List<BillDetailVM>> GetAllBillDetail();
        public Task<BillDetail> GetBillDetailById(Guid id);
        public Task<bool> CreateBillDetailStatus0(Guid id);
        public Task<List<BillDetailView>> ListBillDetail();
    }
}
