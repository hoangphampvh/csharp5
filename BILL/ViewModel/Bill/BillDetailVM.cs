using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.ViewModel.Bill
{
    public class BillDetailVM
    {
        public string? CodeBill { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid BillID { get; set; }
        public Guid ProductID { get; set; }
        public int Status { get; set; }
    }
}
