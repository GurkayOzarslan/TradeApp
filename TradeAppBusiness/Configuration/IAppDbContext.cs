using Microsoft.EntityFrameworkCore;
using TradeAppEntity;

namespace TradeAppApplication
{
    public interface IAppDbContext
    {
        DbSet<Users> Users { get; }
        DbSet<UserPasswords> UserPasswords { get; }
        DbSet<UserRoles> UserRoles { get; }
        DbSet<Roles> Roles { get; }
        DbSet<VerificationCode> VerificationCodes { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
