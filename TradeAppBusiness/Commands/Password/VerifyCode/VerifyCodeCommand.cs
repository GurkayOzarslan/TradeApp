using Microsoft.EntityFrameworkCore;
using TradeAppSharedKernel.Application;

namespace TradeAppApplication.Commands.Password.VerifyCode
{
    public class VerifyCodeCommand : ICommand<bool>
    {
        public long Code { get; set; }
        public string Email { get; set; }
        public VerifyCodeCommand(long code, string email)
        {
            Code = code;
            Email = email;
        }
    }
    public class VerifyCodeCommandHandler : ICommandHandler<VerifyCodeCommand, bool>
    {
        private readonly IAppDbContext _context;

        public VerifyCodeCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(VerifyCodeCommand command, CancellationToken cancellationToken)
        {
            var codeEntry = await _context.VerificationCodes
                .Where(vc => vc.Email == command.Email && vc.Code == command.Code && !vc.IsUsed && vc.ExpireAt > DateTime.UtcNow)
                .FirstOrDefaultAsync(cancellationToken);

            if (codeEntry == null) return false;

            codeEntry.IsUsed = true;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
