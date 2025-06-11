using Microsoft.EntityFrameworkCore;
using TradeAppEntity;

namespace TradeAppApplication
{
    public interface IAppDbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<UserPasswords> UserPasswords { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }
        public DbSet<UserSymbols> UserSymbols { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<ModelPredictions> ModelPredictions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
