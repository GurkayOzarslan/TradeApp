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
        public List<string> Symbols { get; set; }

        public AddNewUserSymbolCommand(List<string> symbols)
        {
            Symbols = symbols;
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

            var symbolIds = command.Symbols.Select(s => s.Trim().ToUpper()).ToList();

            var matchedSymbols = await _context.Symbols
                        .Where(s => symbolIds.Contains(s.Symbol.ToUpper()))
                        .ToListAsync(cancellationToken);

            var existingUserSymbolIds = await _context.UserSymbols
                        .Where(us => us.UserId == userId && matchedSymbols.Select(ms => ms.SymbolId).Contains(us.SymbolId))
                        .Select(us => us.SymbolId)
                        .ToListAsync(cancellationToken);

            var newSymbolsToAdd = matchedSymbols
                        .Where(ms => !existingUserSymbolIds.Contains(ms.SymbolId))
                        .ToList();

            foreach (var symbol in newSymbolsToAdd)
            {
                var newUserSymbol = new UserSymbols(userId, symbol.SymbolId);
                _context.UserSymbols.Add(newUserSymbol);
            }


            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }


}
