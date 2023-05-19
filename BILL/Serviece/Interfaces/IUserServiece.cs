using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    internal interface IUserServiece
    {
        public bool CreatUser(User p);
        public bool EditUser(Guid id, User p);
        public bool DelUser(Guid id);
        public List<User> GetAllUser();

        public User GetUserById(Guid id);
    }
}
