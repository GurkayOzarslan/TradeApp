using System.Security.Cryptography;
using System.Text;
using TradeAppSharedKernel.Application;

namespace TradeAppSharedKernel.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password, out string salt)
        {
            salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            var combined = salt + password;
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return Convert.ToBase64String(hash);
        }

        public bool Verify(string password, string hash, string salt)
        {
            var combined = salt + password;
            using var sha256 = SHA256.Create();
            var computedHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            var computedHashString = Convert.ToBase64String(computedHash);
            return hash == computedHashString;
        }
    }
}
