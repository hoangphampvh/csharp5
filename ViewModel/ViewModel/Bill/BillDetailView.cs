using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModel.Bill
{
    public class BillDetailView
    {
        public Guid ID { get; set; }
        public string? CodeBill { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }      
        public int Status { get; set; }
        public Guid BillID { get; set; }
        public Guid ProductID { get; set; }
        public string NameProduct { get; set; }
        public string UserName { get; set; } 
    }
}
