namespace ASMC5.Models
{
    public class Token
    {
        public string RefreshToken { get; set; }
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public virtual User User { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsActive { get; set; }
        public DateTime IsCreated { get; set; }
        public DateTime Expired { get; set; } // het han khi nao
        public DateTime Iaced { get; set; }
    }
}
