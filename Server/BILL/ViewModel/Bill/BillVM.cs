using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.ViewModel.Bill
{
    public class BillVM
    {
        public DateTime DateCreatBill { get; set; }
        public DateTime DateOfPayment { get; set; }
        public int Status { get; set; }
        public Guid UserID { get; set; }
        public Guid Id { get; set; } = new Guid();
    }
}
