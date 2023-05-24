namespace BILL.ViewModel.Account
{
    public class SignUpVM
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public DateTime Dateofbirth { get; set; }
        public int Status { get; set; }
    }
}
