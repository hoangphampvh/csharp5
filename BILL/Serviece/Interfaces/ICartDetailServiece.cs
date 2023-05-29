using ASMC5.Models;
using BILL.ViewModel.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    public interface ICartDetailServiece
    {
        public Task<bool> CreatCartDetail(CartDetailVM p);
        public Task<bool> EditCartDetail(Guid id, CartDetailVM p);
        public Task<bool> DelCartDetail(Guid id);
        public Task<List<CartDetail>> GetAllCartDetail();

        public Task<CartDetail> GetCartDetailById(Guid id);
    }
}
