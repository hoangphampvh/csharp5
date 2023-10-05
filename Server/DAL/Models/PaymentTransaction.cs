using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    //Giao dịch thanh toán
    public class PaymentTransaction
    {
        public string Id { get; set; } = string.Empty;
        public string? TranMessage { get; set; } = string.Empty;
        // nhận kq thanh toán 
        public string? TranPayload { get; set; } = string.Empty;
        public string? TranStatus { get; set; } = string.Empty;

        // số lượng giao dịch
        public decimal? TranAmount { get; set; }
        public DateTime? TranDate { get; set; }

        [ForeignKey(nameof(PaymentId))]
        public string? PaymentId { get; set; } = string.Empty;
        public Payment payment { get; set; }

    }
}
