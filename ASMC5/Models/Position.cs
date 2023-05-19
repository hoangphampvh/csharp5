using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASMC5.Models
{
    public class Position : IdentityRole<Guid>
    {
        public int status { get; set; }
    }
}
