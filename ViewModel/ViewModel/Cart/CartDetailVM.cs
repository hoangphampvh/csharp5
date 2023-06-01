using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.ViewModel.Cart
{
    public class CartDetailVM
    {
        public int Quantity { get; set; }

        public Guid CartID { get; set; }
        public int Status { get; set; }
        public Guid ProductID { get; set; }

    }
}
