using ASMC5.Models;
using BILL.ViewModel.Account;
using BILL.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    public interface IUserServiece
    {
        public Task<bool> DelUsers(List<Guid> Ids);
        public Task<bool> CreatUser(UserCreateVM p);
        public Task<bool> EditUser(Guid id, UserEditVM p);
        public Task<bool> DelUser(Guid id);
        public Task<List<User>> GetAllUser();
        public Task<List<User>> GetAllUserActive();
        public Task<User> GetUserById(Guid id);
        public Task<LoginResponesVM> LoginWithJWT(LoginRequestVM loginRequest);
        public Task<bool> SignUp(SignUpVM p);
    }
}
