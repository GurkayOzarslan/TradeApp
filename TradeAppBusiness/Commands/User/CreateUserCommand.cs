using TradeAppEntity;
using TradeAppSharedKernel.Application;

namespace TradeAppApplication.Commands.User
{
    public class CreateUserCommand : CommandBase<string>
    {
        public string Name { get; set; }
        public string? MiddleName { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, string>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IAppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var passwordHash = _passwordHasher.Hash(request.Password, out var salt);

                var user = new Users(
                    name: request.Name,
                    middleName: request.MiddleName,
                    surname: request.Surname,
                    username: request.Username,
                    email: request.Email,
                    phoneNumber: request.PhoneNumber
                );

                user.Passwords.Add(new UserPasswords(passwordHash, salt));
                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);

                //await _context.SaveChangesAsync(cancellationToken);

                return "GG";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
    }
}
