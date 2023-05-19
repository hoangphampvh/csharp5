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
        public bool CreatBill(Bill p);
        public bool EditBill(Guid id, Bill p);
        public bool DelBill(Guid id);
        public List<Bill> GetAllBill();

        public Bill GetBillById(Guid id);
    }
}
