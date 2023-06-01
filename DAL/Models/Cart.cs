using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASMC5.Models
{
    public class Cart
    {
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public int Status {get; set; }
        
        public virtual User User { get; set; }
        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }
}
