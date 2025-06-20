using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TradeAppApplication.Responses.User;
using TradeAppSharedKernel.Application;
using TradeAppSharedKernel.Infrastructure.Helpers.TokenInfo;
using YahooFinanceApi;

namespace TradeAppApplication.Queries.User.GetUserSignal
{
    public class GetUserSignalQuery : IQuery<UserSignalResponse>
    {
    }

    public class GetUserSignalQueryHandler : IQueryHandler<GetUserSignalQuery, UserSignalResponse>
    {
        private readonly IAppDbContext _context;
        private readonly ITokenInfoHandler _tokenInfoHandler;
        private readonly IMapper _mapper;

        public GetUserSignalQueryHandler(IAppDbContext context, ITokenInfoHandler tokenInfoHandler, IMapper mapper)
        {
            _context = context;
            _tokenInfoHandler = tokenInfoHandler;
            _mapper = mapper;
        }

        public async Task<UserSignalResponse> Handle(GetUserSignalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tokenInfo = _tokenInfoHandler.TokenInfo();

                var userSignals = await _context.UserSignals.Where(x => x.UserId == tokenInfo.UserId).ProjectTo<UserSignalResponse>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

                var userSignalSymbol = await _context.UserSignals
                    .Include(x => x.User)
                    .ThenInclude(y => y.UserSymbols)
                    .ThenInclude(z => z.Symbols)
                    .Where(x => x.UserId == tokenInfo.UserId)
                    .FirstOrDefaultAsync(cancellationToken);

                var userSignalSymbol2 = await _context.UserSignals
                       .Include(x => x.User)
                       .ThenInclude(y => y.UserSymbols)
                       .ThenInclude(z => z.Symbols)
                       .Where(x => x.UserId == tokenInfo.UserId)
                       .Select(s => new
                       {
                           Id = s.Id,
                           UserId = s.UserId,
                           SymbolId = s.SymbolId,
                           SymbolName = s.Symbol.Symbol
                       }).FirstOrDefaultAsync(cancellationToken);

                var signalResponse = new List<UserSignalResponse>();


                var securities = await Yahoo
                    .Symbols(userSignalSymbol2.SymbolName)
                    .Fields(
                            Field.Symbol,
                            Field.RegularMarketPrice,
                            Field.RegularMarketChangePercent,
                            Field.RegularMarketPreviousClose)
                .QueryAsync();

                if (securities.TryGetValue(userSignalSymbol2.SymbolName, out var data))
                {
                    var currentPrice = (decimal)(data[Field.RegularMarketPrice] ?? 0m);
                    var previousClose = (decimal)(data[Field.RegularMarketPreviousClose] ?? 0m);
                    var dailyChange = currentPrice - previousClose;

                    signalResponse.Add(new UserSignalResponse
                    {
                        Symbol = "asdad",
                        Price = (decimal)(data[Field.RegularMarketPrice] ?? 0m),
                        ChangePercent = Math.Round((decimal)(data[Field.RegularMarketChangePercent] ?? 0m), 2),
                        ChangePrice = dailyChange
                    });
                }

                if (userSignals != null)
                {
                    return userSignals;
                }
                else
                {
                    throw new NullReferenceException("Bu kullanıcının kaydı bulunmamaktadır");
                }
            }
            catch (Exception ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }
    }
}
