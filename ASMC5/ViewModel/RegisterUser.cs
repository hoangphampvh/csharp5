using static ServiceStack.LicenseUtils;
using System.ComponentModel.DataAnnotations;

namespace ASMC5.ViewModel
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Username cannot be blank")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password cannot be blank")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm password cannot be blank")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Address cannot be blank")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "number phone cannot be blank")]
        public string? numberPhone { get; set; }

        [Required(ErrorMessage = "dateofbirth cannot be blank")]
        public DateTime? dateofbirth { get; set; }


    }
}
