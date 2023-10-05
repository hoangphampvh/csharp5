using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{

    //Chữ ký thanh toán
    public class PaymentSignature
    {
        public string Id { get; set; } = string.Empty;
        
        public string? SignValue { get; set; } = string.Empty;
        public string? SignAlgo { get; set; } = string.Empty;
        // chữ ký của bên nào
        public string? SignOwn { get; set; } = string.Empty;
        public DateTime? SignDate { get; set; }
        public bool IsValid { get; set; }

        [ForeignKey(nameof(PaymentId))]
        public string? PaymentId { get; set; } = string.Empty;
        public Payment Payment { get; set; }
    }
}
