using DAL.common;
using DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASMC5.Models
{
    public class Merchant : BaseAuditableEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string? MerchantName { get; set; } = string.Empty;
        public string? MerchantWebLink { get; set; } = string.Empty;

        // url cập nhận kết quả thanh toán (Backend)
        public string? MerchantIpnUrl { get; set; } = string.Empty;

        // url trả kết quả thanh toán (forntend)

        public string? MerchantReturnUrl { get; set; } = string.Empty;
        public string? SecretKey { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
