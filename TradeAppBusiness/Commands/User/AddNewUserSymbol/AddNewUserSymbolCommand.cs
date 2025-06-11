using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TradeAppEntity;
using TradeAppSharedKernel.Application;
using TradeAppSharedKernel.Infrastructure.Helpers.TokenInfo;

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
        private readonly ITokenInfoHandler _tokenInfoHandler;

        public AddNewSymbolCommandHandler(IAppDbContext context, IHttpContextAccessor httpContextAccessor, ITokenInfoHandler tokenInfoHandler)
        {
            _context = context;
            _tokenInfoHandler = tokenInfoHandler;
        }
        public async Task<bool> Handle(AddNewUserSymbolCommand command, CancellationToken cancellationToken)
        {
            var tokenInfo = _tokenInfoHandler.TokenInfo();
            var userId = tokenInfo.NameIdentifier;
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
