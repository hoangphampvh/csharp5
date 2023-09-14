using ASMC5.Models;
using BILL.ViewModel;
using BILL.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    public interface IAccountService
    {
        public Task<bool> SignUp(SignUpVM p); 
        public Task<LoginResponesVM> RenewToken(TokenModel tokenDTO);  
        public Task<LoginResponesVM> Validate(LoginRequestVM loginRequest);

    }
}
