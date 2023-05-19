using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASMC5.Models
{
    public class BillDetail
    {

        public Guid ID { get; set; }
        public string? CodeBill { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid BillID { get; set; }
        public int Status { get; set; }
        public virtual Bill Bill { get; set; }
        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; }


    }
}
