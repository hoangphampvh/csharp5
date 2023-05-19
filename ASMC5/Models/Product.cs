using System.ComponentModel.DataAnnotations;

namespace ASMC5.Models
{
    public class Product
    {

        public Guid ID { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public string? Supplier { get; set; }
        public string? Description { get; set; }
        public string? UrlImage { get; set; }
        public DateTime? CreateDate { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set;}
        public virtual ICollection<CartDetail> CartDetails { get; set;} 
    }
}
