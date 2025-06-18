using TradeAppApplication.Responses.Stock;
using TradeAppSharedKernel.Application;
using TradeAppSharedKernel.Infrastructure.Helpers.TokenInfo;
using YahooFinanceApi;

namespace TradeAppApplication.Queries.Stock.GetFreeStockList
{
    public class GetStockListQuery : QueryBase<List<FreeStockResponseList>>
    {
    }
    public class GetStockListQuerytHandler : IQueryHandler<GetStockListQuery, List<FreeStockResponseList>>
    {
        private readonly IAppDbContext _context;
        private readonly ITokenInfoHandler _tokenInfoHandler;
        public GetStockListQuerytHandler(IAppDbContext context, ITokenInfoHandler tokenInfoHandler)
        {
            _context = context;
            _tokenInfoHandler = tokenInfoHandler;
        }
        public async Task<List<FreeStockResponseList>> Handle(GetStockListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tokenInfo = _tokenInfoHandler.TokenInfo();


                var userSymbols = _context.UserSymbols
                    .Where(us => us.UserId == tokenInfo.NameIdentifier)
                    .Select(us => us.Symbols.Symbol)
                    .ToList();


                var stocks = new List<FreeStockResponseList>();

                var securities = await Yahoo
                    .Symbols(userSymbols.ToArray())
                    .Fields(
                            Field.Symbol,
                            Field.RegularMarketPrice,
                            Field.RegularMarketChangePercent,
                            Field.RegularMarketPreviousClose)
                    .QueryAsync();

                foreach (var symbol in userSymbols)
                {
                    if (securities.TryGetValue(symbol, out var data))
                    {
                        var currentPrice = (decimal)(data[Field.RegularMarketPrice] ?? 0m);
                        var previousClose = (decimal)(data[Field.RegularMarketPreviousClose] ?? 0m);
                        var dailyChange = currentPrice - previousClose;

                        stocks.Add(new FreeStockResponseList
                        {
                            Symbol = symbol,
                            Price = (decimal)(data[Field.RegularMarketPrice] ?? 0m),
                            ChangePercent = Math.Round((decimal)(data[Field.RegularMarketChangePercent] ?? 0m), 2),
                            ChangePrice = dailyChange
                        });
                    }

                }
                return stocks;
            }
            catch (Exception ex)
            {
                throw new Exception("Hisse senedi bulunamadı veya veri alınamadı.");
            }
        }
    }
}
