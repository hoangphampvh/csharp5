using ASMC5.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Interfaces
{
    public interface ICurrentUserProvider
    {
        Task<UserVM> GetCurrentUserInfo();
        UserVM GetCurrentUserInfos();
        public string? IpAddress { get; }
    }
}
