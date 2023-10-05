using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModel.PaymentConfiguration.MerchantVM
{
    public class SetActivePaymentDestinationVM
    {
        public string Id { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
