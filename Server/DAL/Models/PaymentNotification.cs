using ASMC5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{

    //Thông báo thanh toán
    public class PaymentNotification
    {
        public string Id { get; set; } = string.Empty;
        
        public DateTime? NotiDate { get; set; }
        public string? NotiContent { get; set; } = string.Empty;
        public decimal NotiAmount { get; set; }
        public string? NotiMessage { get; set; } = string.Empty;

        // thông báo chữ ký
        public string? NotiSignature { get; set; } = string.Empty;
        public string? NotiPaymentId { get; set; } = string.Empty;
        public string? NotiMerchantId { get; set; } = string.Empty;
        public string? NotiNotiStatus { get; set; } = string.Empty;
        public DateTime? NotiResDate { get; set; }
        public string? NotiResMessage { get; set; } = string.Empty;
        public string? NotiResHttpCode { get; set; } = string.Empty;
        [ForeignKey(nameof(PaymentRefId))]
        public string PaymentRefId { get; set; } = string.Empty;
        public Payment payment { get; set; }
    }
}
