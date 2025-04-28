using TradeAppSharedKernel.Application;

namespace TradeAppApplication.Commands.Password.ForgotPassowrd
{
    public class ForgotPasswordCommand : ICommand<bool>
    {
        public string Email { get; set; }
        public ForgotPasswordCommand(string email)
        {
            Email = email;
        }
    }
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, bool>
    {
        private readonly IAppDbContext _context;

        public ForgotPasswordCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public Task<bool> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
