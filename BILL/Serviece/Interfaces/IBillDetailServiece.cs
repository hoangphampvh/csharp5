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
        public bool CreatBillDetail(BillDetail p);
        public bool EditBillDetail(Guid id, BillDetail p);
        public bool DelBillDetail(Guid id);
        public List<BillDetail> GetAllBillDetail();

        public BillDetail GetBillDetailById(Guid id);
    }
}
