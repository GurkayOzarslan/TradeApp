using Microsoft.EntityFrameworkCore;
using TradeAppEntity;
using TradeAppSharedKernel.Application;

namespace TradeAppApplication.Commands.Password.GenerateVerificationCode
{
    public class GenerateVerificationCodeCommand :ICommand<bool>
    {
        public string Email { get; set; }
        public GenerateVerificationCodeCommand(string email)
        {
            Email = email;
        }
    }
    public class GenerateVerificationCodeCommandHandler : ICommandHandler<GenerateVerificationCodeCommand, bool>
    {
        private readonly IAppDbContext _context;
        //private readonly IEmailSender _emailSender; // SMTP ya da SendGrid gibi

        public GenerateVerificationCodeCommandHandler(IAppDbContext context)
        {
            _context = context;
            //_emailSender = emailSender;
        }

        public async Task<bool> Handle(GenerateVerificationCodeCommand command, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == command.Email);
            if (user == null) return false;

            var code = new Random().Next(100000, 999999); 
            var expireAt = DateTime.UtcNow.AddMinutes(3); 
            var verification = new VerificationCode
            {
                Email = command.Email,
                Code = code,
                ExpireAt = expireAt
            };

            _context.VerificationCodes.Add(verification);
            await _context.SaveChangesAsync(cancellationToken);

            //await _emailSender.SendAsync(command.Email, "Doğrulama Kodunuz", $"Kodunuz: {code}");

            return true;
        }
    }
}
