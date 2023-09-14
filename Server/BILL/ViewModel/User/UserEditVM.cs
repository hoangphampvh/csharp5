using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.ViewModel.User
{
    public class UserEditVM
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
        public string? UrlImage { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public DateTime Dateofbirth { get; set; }
        public int Status { get; set; }
        public List<string>? roleNames { get; set; }
    }
}
