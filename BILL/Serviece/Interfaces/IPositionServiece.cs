using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    public interface IPositionServiece
    {
        public Task<bool> CreatPosition(Position p);

        public Task<bool> EditPosition(Guid id, int status, string name, string NormalizedName, string ConcurrencyStamp);
        public Task<bool> DelPosition(Guid id);
        public Task<List<Position>> GetAllPosition();
        public Task<List<Position>> GetAllPositionActive();
        public Task<Position> GetPositionById(Guid id);
    }
}
