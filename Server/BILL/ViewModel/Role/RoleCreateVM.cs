using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModel.Role
{
    public class RoleCreateVM
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? normalizedName { get; set; }

        public string? concurrencyStamp { get; set; }

    }
}
