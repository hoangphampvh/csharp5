using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    internal interface IPositionServiece
    {
        public bool CreatPosition(Position p);
        public bool EditPosition(Guid id, Position p);
        public bool DelPosition(Guid id);
        public List<Position> GetAllPosition();

        public Position GetPositionById(Guid id);
    }
}
