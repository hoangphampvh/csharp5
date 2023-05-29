using ASMC5.Models;
using BILL.ViewModel.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    public interface ICartServiece
    {
        public Task<bool> CreatCart(CartVN p); 
        public Task<bool> EditCart(Guid id, CartVN p); 
        public Task<bool> DelCart(Guid id); 
        public Task<List<Cart>> GetAllCart(); 
        public Task<Cart> GetCartById(Guid id);

    }
}
