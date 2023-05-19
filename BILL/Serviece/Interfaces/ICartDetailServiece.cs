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
        public bool CreatCartDetail(CartDetail p);
        public bool EditCartDetail(Guid id, CartDetail p);
        public bool DelCartDetail(Guid id);
        public List<CartDetail> GetAllCartDetail();

        public CartDetail GetCartDetailById(Guid id);
    }
}
