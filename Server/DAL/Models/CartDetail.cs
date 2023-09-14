using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASMC5.Models
{
    public class CartDetail
    {

        public Guid ID { get; set; }     
        public int Quantity { get; set; }

        public Guid UserID { get; set; }
        public Cart Cart { get; set; } 
        public int Status { get; set; }
        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
