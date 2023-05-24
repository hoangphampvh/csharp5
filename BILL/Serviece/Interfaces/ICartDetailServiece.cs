using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    internal interface ICartDetailServiece
    {
        public Task<bool> CreatCartDetail(CartDetail p);
        public Task<bool> EditCartDetail(Guid id, CartDetail p);
        public Task<bool> DelCartDetail(Guid id);
        public Task<List<CartDetail>> GetAllCartDetail();

        public Task<CartDetail> GetCartDetailById(Guid id);
    }
}
