using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    public interface IUserServiece
    {
        public Task<bool> CreatUser(User p);
        public Task<bool> EditUser(Guid id, User p);
        public Task<bool> DelUser(Guid id);
        public Task<List<User>> GetAllUser();

        public Task<User> GetUserById(Guid id);
    }
}
