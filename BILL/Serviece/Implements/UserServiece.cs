using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BILL.Serviece.Implements
{
    public class UserServiece : IUserServiece
    {
        private readonly ASMDBContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public UserServiece(ASMDBContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> CreatUser(User p)
        {
            try
            {
                var user = new User()
                {
                    Id = new Guid(),
                    UserName = p.UserName,
                    Password = p.Password,
                    PhoneNumber = p.PhoneNumber,
                    Dateofbirth = p.Dateofbirth,
                    Status = 0,   // quy uoc 0 có nghĩa là đang hđ
                    DiaChi = p.DiaChi,
                    Email = p.Email,

                };
                var result = await _userManager.CreateAsync(user,p.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user,"client");
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task <bool> DelUser(Guid id)
        {
            try
            {
                var obj = await _context.Users.ToListAsync();
                var user = obj.FirstOrDefault(c=>c.Id == id);
                user.Status = 1;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> EditUser(Guid id, User p)
        {
            try
            {
                var listobj = _context.Users.ToList();
                var obj = listobj.FirstOrDefault(c => c.Id == id);
                obj.DiaChi = p.DiaChi;
                obj.Password = p.Password;               
                obj.Status = p.Status;
                _context.Users.Update(obj);
                await _context.SaveChangesAsync();
                return true;

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

        public async Task<User> GetUserById(Guid id)
        {
            var list = await _context.Users.AsQueryable().ToListAsync();
            return list.FirstOrDefault(c => c.Id == id);
        }
    }
}
