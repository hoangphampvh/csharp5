using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModel.Bill
{
    public class BillView
    {
        public Guid BillID { get; set; }
        public DateTime DateCreatBill { get; set; }
        public DateTime DateOfPayment { get; set; }
        public int Status { get; set; }
        public string UserName { get; set; }
    }
}
