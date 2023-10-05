using DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASMC5.Models
{
    public class Payment
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        // content
        public string PaymentContent { get; set; } = string.Empty;
        
        // tiền tệ thanh toán
        public string PaymentCurrency { get; set; } = string.Empty;

        // mã tham chiếu đến đơn hàng đến website merchant
        public string PaymentRefId { get; set; } = string.Empty;

        // khối lượng bắt buộc
        public decimal? RequiredAmount { get; set; }
        public DateTime? PaymentDate { get; set; } = DateTime.Now;
        public DateTime? ExpireDate { get; set; }
        public string? PaymentLanguage { get; set; } = string.Empty;
        public decimal? PaidAmount { get; set; }
        public string? PaymentStatus { get; set; } = string.Empty;
        public string? Signature { get; set; } = string.Empty;
        public string? PaymentLastMessage { get; set; } = string.Empty;
        public virtual ICollection<CartDetail> CartDetails { get; set; }
        public virtual ICollection<PaymentSignature> PaymentSignatures { get; set; }
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        public virtual ICollection<PaymentNotification> PaymentNotifications { get; set; }

        [ForeignKey(nameof(PaymentDestinationId))]
        public string? PaymentDestinationId { get; set; } = string.Empty;
        public PaymentDestination PaymentDestination { get; set; }

        [ForeignKey(nameof(MerchantId))]
        public string? MerchantId { get; set; } = string.Empty;
        public Merchant Merchant { get; set; }



    }
}
