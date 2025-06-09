using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TradeAppEntity;
using TradeAppSharedKernel.Application;

namespace TradeAppApplication.Commands.User.AddNewUserSymbol
{
    public class AddNewUserSymbolCommand : ICommand<bool>
    {
        public string Symbol { get; set; }
        public AddNewUserSymbolCommand(string symbol)
        {
            Symbol = symbol;
        }
    }

    public class AddNewSymbolCommandHandler : ICommandHandler<AddNewUserSymbolCommand, bool>
    {
        private readonly IAppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddNewSymbolCommandHandler(IAppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Handle(AddNewUserSymbolCommand command, CancellationToken cancellationToken)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var existingSymbol = await _context.UserSymbols
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Symbol == command.Symbol, cancellationToken);
            if (existingSymbol != null)
            {
                throw new ApplicationException("This symbol already exists for the user.");
            }
            var newSymbol = new UserSymbols
            (
                userId: userId,
                symbol: command.Symbol
            );
            _context.UserSymbols.Add(newSymbol);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }


}
