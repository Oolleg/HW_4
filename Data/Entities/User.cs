namespace HW_4.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public String? Fullname { get; set; }
        public String Email { get; set; } = null!;
        public String Phone { get; set; } = null!;
        public String Login { get; set; } = null!;
        public String PasswordSalt { get; set; } = null!;
        public String PasswordDk { get; set; } = null!;
        public String? Avatar { get; set; }
        public DateTime RegisterDt { get; set; }
        public DateTime? DeleteDt { get; set; }
    }
}
