using System.Security.Claims;

namespace TradeAppSharedKernel.Infrastructure.Helpers.Token
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(int userId, string email, string username, string name, string middleName, string surname, List<string> roles);
        bool ValidateToken(string token);
    }
}
