using DAL.common;
using DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASMC5.Models
{

    // điểm đến thanh toán
    public class PaymentDestination : BaseAuditableEntity
    {
        public string Id { get; set; } = string.Empty;
        public string? DesName { get; set; } = string.Empty;
        public string? DesShortName { get; set; } = string.Empty;
        
        public string? DesLogo { get; set; } = string.Empty;
        public int SortIndex { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey(nameof(DesParentId))]
        public string? DesParentId { get; set; } = string.Empty;
        public virtual ICollection<PaymentDestination> PaymentDestinations { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

       

    }
}
