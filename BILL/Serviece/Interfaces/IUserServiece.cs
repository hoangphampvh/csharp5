using ASMC5.Models;
using ASMC5.ViewModel;
using BILL.ViewModel.Account;
using BILL.ViewModel.User;


namespace BILL.Serviece.Interfaces
{
    public interface IUserServiece
    {
        public Task<bool> DelUsers(List<Guid> Ids);
        public Task<bool> CreatUser(UserCreateVM p);
        public Task<bool> EditUser(Guid id, UserEditVM p);
        public Task<bool> DelUser(Guid id);
        public Task<List<UserVM>> GetAllUser();
        public Task<List<UserVM>> GetAllUserActive();
        public Task<User> GetUserById(Guid id);
        public Task<UserVM> GetUserByName(string name);
        public Task<LoginResponesVM> LoginWithJWT(LoginRequestVM loginRequest);
        public Task<bool> SignUp(SignUpVM p);

    }
}
