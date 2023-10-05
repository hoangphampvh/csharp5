using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModel.PaymentConfiguration.PaymentVM.Payment
{
    public class VnpayPayIpnResponseVM
    {

        public VnpayPayIpnResponseVM()
        {

        }
        public VnpayPayIpnResponseVM(string rspCode, string message)
        {
            RspCode = rspCode;
            Message = message;
        }
        public void Set(string rspCode, string message)
        {
            RspCode = rspCode;
            Message = message;
        }
        public string RspCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
