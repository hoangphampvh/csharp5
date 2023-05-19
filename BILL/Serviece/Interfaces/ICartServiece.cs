using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    internal interface ICartServiece
    {
        public bool CreatCart(Cart p);
        public bool EditCart(Guid id, Cart p);
        public bool DelCart(Guid id);
        public List<Cart> GetAllCart();

        public Cart GetCartById(Guid id);
    }
}
