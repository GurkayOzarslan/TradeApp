using System.Text.Json.Serialization;

namespace TradeAppEntity
{
    public class UserPasswords2
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string PasswordHash { get; private set; }
        public string Salt { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        [JsonIgnore]
        public Users User { get; private set; }


        private UserPasswords2() { }

        // Şifre oluşturma metodu
        public UserPasswords2(string passwordHash, string salt)
        {
            PasswordHash = passwordHash;
            Salt = salt;
        }

        // Şifreyi güncelleme metodu
        public void UpdatePassword(string newPasswordHash, string newSalt)
        {
            PasswordHash = newPasswordHash;
            Salt = newSalt;
            UpdatedAt = DateTime.UtcNow;
        }
        public void SoftDelete()
        {
            DeletedAt = DateTime.UtcNow;
        }
    }
}
