using TradeAppApplication.Responses.Stock;
using TradeAppSharedKernel.Application;
using YahooFinanceApi;

namespace TradeAppApplication.Queries.Stock.GetFreeStockList
{
    public class GetFreeStockQuery : QueryBase<List<FreeStockResponseList>>
    {
        public List<string> Symbol { get; set; }

        public GetFreeStockQuery(List<string> symbol)
        {
            Symbol = symbol;
        }
    }
    public class GetLoginUserQueryHandler : IQueryHandler<GetFreeStockQuery, List<FreeStockResponseList>>
    {
        public async Task<List<FreeStockResponseList>> Handle(GetFreeStockQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var stocks = new List<FreeStockResponseList>();

                var securities = await Yahoo
                    .Symbols(request.Symbol.ToArray())
                    .Fields(
                            Field.Symbol,
                            Field.RegularMarketPrice,
                            Field.RegularMarketChangePercent,
                            Field.RegularMarketPreviousClose)
                    .QueryAsync();

                foreach (var symbol in request.Symbol)
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
