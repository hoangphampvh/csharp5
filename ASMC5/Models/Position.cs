using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASMC5.Models
{
    public class Position : IdentityRole<Guid>
    {
        public bool status { get; set; }
    }
}
