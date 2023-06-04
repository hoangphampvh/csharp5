using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModel.Role;

namespace BILL.Serviece.Interfaces
{
    public interface IPositionServiece
    {
        public Task<bool> CreatPosition(RoleCreateVM p);

        public Task<bool> EditPosition(Guid id, RoleUpdateVM roleUpdate);
        public Task<bool> DelPosition(Guid id);
        public Task<List<Position>> GetAllPosition();
        public Task<List<Position>> GetAllPositionActive();
        public Task<Position> GetPositionById(Guid id);
    }
}
