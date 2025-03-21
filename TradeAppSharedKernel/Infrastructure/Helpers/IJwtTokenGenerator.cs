namespace TradeAppSharedKernel.Infrastructure.Helpers
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(int userId, string email, string username, string name, string middleName, string surname, List<string> roles);
    }
}
