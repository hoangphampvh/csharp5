using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Account;
using BILL.ViewModel.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BILL.Serviece.Implements
{
    public class UserServiece : IUserServiece
    {
        private readonly ASMDBContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<Position> _roleManager;
        public UserServiece(UserManager<User> userManager, SignInManager<User> signInManager,
            IConfiguration configuration, RoleManager<Position> roleManager)
        {
            _context = new ASMDBContext();
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        public async Task<LoginResponesVM> LoginWithJWT(LoginRequestVM loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.UserName);
            if (user == null) return new LoginResponesVM
            {
                Successful = false,
                Error = "Người dùng không tồn tại trong hệ thống."
            };
            var login = await _signInManager.PasswordSignInAsync(user, user.Password, false, false);
            if (!login.Succeeded) return new LoginResponesVM
            {
                Successful = false,
                Error = "Người dùng không tồn tại trong hệ thống."
            };
            var rolesOfUser = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,string.Join(";",rolesOfUser))
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKeys"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
              _configuration["JwtIssuer"],
              _configuration["JwtAudience"],
              claims,
              expires: DateTime.Now.AddHours(3),
              signingCredentials: creds);
            LoginResponesVM loginResponesVm = new LoginResponesVM { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) };
            return loginResponesVm;

        }
      

        public async Task<bool> SignUp(SignUpVM p)
        {
            try
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = p.UserName,
                    PhoneNumber = p.PhoneNumber,
                    Dateofbirth = p.Dateofbirth,
                    Status = 0,   // quy uoc 0 có nghĩa là đang hđ
                    DiaChi = p.DiaChi,
                    Email = p.Email,
                    Password = p.Password,

                };
                var result = await _userManager.CreateAsync(user, p.Password);
                if (result.Succeeded)
                {

                   await _userManager.AddToRoleAsync(user,"Client");
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<bool> DelUsers(List<Guid> Ids)
        {
            try
            {
                List<User> Users = new List<User>();
                foreach (var id in Ids)
                {
                    Users = await _context.Users.Where(p => p.Id == id).ToListAsync();
                }


                foreach (var item in Users)
                {
                    item.Status = 1;
                }
                _context.Users.UpdateRange(Users);
                await _context.SaveChangesAsync();

                //obj.Status = 1; // ta sẽ kh xóa mà thay đổi trạng thái từ hđ sang kh hđ
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<bool> CreatUser(UserCreateVM p)
        {
            try
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    UrlImage = p.UrlImage,
                    UserName = p.UserName,
                    PhoneNumber = p.PhoneNumber,
                    Dateofbirth = p.Dateofbirth,
                    Status = 0,   // quy uoc 0 có nghĩa là đang hđ
                    DiaChi = p.DiaChi,
                    Email = p.Email,
                    Password = p.Password,

                };
                var result = await _userManager.CreateAsync(user, p.Password);
                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user,"Client");
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> DelUser(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                user.Status = 1;
                _context.Users.Update(user);
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> EditUser(Guid id, UserEditVM p)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                user.UserName = p.UserName;
                user.DiaChi = p.DiaChi;
                user.Password = p.Password;
                user.Status = p.Status;
                user.UrlImage = p.UrlImage;
                user.Dateofbirth = p.Dateofbirth;
                user.PhoneNumber = p.PhoneNumber;
                user.Email = p.Email;

                // Lấy các vai trò hiện tại của người dùng
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Chuyển đổi roleIds sang dạng mảng string
                var roleNameArray = p.roleNames.Select(x => x.ToString()).ToArray();

                // Tìm các vai trò cần thêm
                var rolesToAdd = roleNameArray.Except(currentRoles);
                foreach (var item in rolesToAdd)
                {
                    Console.WriteLine(item);
                }
                // Tìm các vai trò cần xóa
                var rolesToRemove = currentRoles.Except(roleNameArray);

                // Thêm các vai trò mới cho người dùng
                await _userManager.AddToRolesAsync(user, rolesToAdd);

                // Xóa các vai trò cần xóa khỏi người dùng
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;


            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<User>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<List<User>> GetAllUserActive()
        {
            return await _context.Users.Where(p => p.Status != 1).ToListAsync();
        }
        public async Task<User> GetUserById(Guid id)
        {
            var list = await _context.Users.AsQueryable().ToListAsync();
            return list.FirstOrDefault(c => c.Id == id);
        }
    }
}
