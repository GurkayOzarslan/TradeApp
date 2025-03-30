using Microsoft.EntityFrameworkCore;
using TradeAppEntity;
using TradeAppSharedKernel.Application;

namespace TradeAppApplication.Commands.Password.ChangePassword
{
    public class ChangePasswordCommand : ICommand<bool>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public ChangePasswordCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, bool>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public ChangePasswordCommandHandler(IAppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users
                            .Include(x => x.Passwords)
                            .FirstOrDefaultAsync(x => x.Email == command.Email, cancellationToken);

                if (user == null)
                    throw new Exception("User not found.");

                var lastPasswords = user.Passwords
                    .OrderByDescending(p => p.CreatedAt)
                    .Where(p => p.DeletedAt == null || p.DeletedAt > DateTime.UtcNow.AddYears(-1))
                    .Take(3)
                    .ToList();

                foreach (var oldPass in lastPasswords)
                {
                    if (_passwordHasher.Verify(command.Password, oldPass.PasswordHash, oldPass.Salt))
                        throw new Exception("New password must not match any of the last 3 used passwords.");
                }

                var currentPassword = lastPasswords.FirstOrDefault(p => p.DeletedAt == null);
                currentPassword?.SoftDelete();

                var newHash = _passwordHasher.Hash(command.Password, out var newSalt);
                user.Passwords.Add(new UserPasswords(newHash, newSalt));

                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Password change failed: {ex.Message}");
            }
        }
    }
}
