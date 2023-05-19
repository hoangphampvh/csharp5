using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Implements
{
    internal class UserServiece : IUserServiece
    {
        ASMDBContext context;
        public UserServiece()
        {
            this.context = new ASMDBContext();
        }
        public bool CreatUser(User p)
        {
            try
            {
                p.Id = Guid.NewGuid();
                context.Add(p);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DelUser(Guid id)
        {
            try
            {
                var list = context.users.ToList();
                var obj = list.FirstOrDefault(c => c.Id == id);
                context.users.Remove(obj);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool EditUser(Guid id, User p)
        {
            try
            {
                var listobj = context.users.ToList();
                var obj = listobj.FirstOrDefault(c => c.Id == id);
                obj.DiaChi = p.DiaChi;
                obj.Password = p.Password;               
                obj.Status = p.Status;
                context.users.Update(obj);
                context.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<User> GetAllUser()
        {
            return context.users.ToList();
        }

        public User GetUserById(Guid id)
        {
            var list = context.users.AsQueryable().ToList();
            return list.FirstOrDefault(c => c.Id == id);
        }
    }
}
