using Microsoft.EntityFrameworkCore;
using TradeAppApplication;
using TradeAppEntity;

namespace TradeAppDataAccess
{
    public class AppDbContext : BaseDbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<UserPasswords> UserPasswords { get; set; }
        public DbSet<UserPasswords2> UserPasswords2 { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }
        public DbSet<UserSymbols> UserSymbols { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<ModelPredictions> ModelPredictions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasMany(u => u.Passwords)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRoles>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRoles>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRoles>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<UserPasswords>()
                .HasOne(up => up.User)
                .WithMany(u => u.Passwords)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPasswords2>()
                .HasOne(up => up.User)
                .WithMany(u => u.Passwords2)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSymbols>()
                .HasKey(us => us.Id);

            modelBuilder.Entity<UserSymbols>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSymbols)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.Users)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ModelPredictions>(entity =>
            {
                entity.HasKey(mp => mp.PredictionId);
                entity.Property(mp => mp.Symbol)
                  .IsRequired()
                  .HasMaxLength(10);
                entity.Property(mp => mp.PredictionValue)
                  .IsRequired()
                  .HasColumnType("decimal(18,4)");
                entity.Property(mp => mp.CreatedAt)
                  .IsRequired();
            });
        }
    }
}
