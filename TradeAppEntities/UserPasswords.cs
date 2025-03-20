namespace TradeAppEntity
{
    public class UserPasswords
    {
        public int UserId { get; private set; }  
        public string PasswordHash { get; private set; }
        public string Salt { get; private set; }
        public DateTime LastChangedAt { get; private set; } = DateTime.UtcNow;

        private UserPasswords() { }

        // Şifre oluşturma metodu
        public UserPasswords(int userId, string passwordHash, string salt)
        {
            UserId = userId;
            PasswordHash = passwordHash;
            Salt = salt;
            LastChangedAt = DateTime.UtcNow;
        }

        // Şifreyi güncelleme metodu
        public void UpdatePassword(string newPasswordHash, string newSalt)
        {
            PasswordHash = newPasswordHash;
            Salt = newSalt;
            LastChangedAt = DateTime.UtcNow;
        }
    }
}
