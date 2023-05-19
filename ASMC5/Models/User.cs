using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASMC5.Models
{
    public class User : IdentityUser<Guid>
    {     
        public string? DiaChi { get; set; }
        public string? Password { get; set; }
        public DateTime Dateofbirth { get; set; }
        public int Status { get; set; }
      
        public virtual Cart CartNavigation { get; set; }
        public virtual ICollection<Bill> Bill { get; set; } 
        
    }
}
