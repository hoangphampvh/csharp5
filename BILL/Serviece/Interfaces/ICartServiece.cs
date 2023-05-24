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
        public Task<bool> CreatCart(Cart p); 
        public Task<bool> EditCart(Guid id, Cart p); 
        public Task<bool> DelCart(Guid id); 
        public Task<List<Cart>> GetAllCart(); 
        public Task<Cart> GetCartById(Guid id);
    }
}
