using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASMC5.Models
{
    public class Bill
    {

        public Guid ID { get; set; }
        public DateTime DateCreatBill { get; set; }
        public DateTime DateOfPayment { get; set; }
        public int Status { get; set; }
        
        public Guid UserID { get; set; }
        public User User { get; set; }

        public virtual ICollection<BillDetail> BillDetail { get; set; }
    }
}
