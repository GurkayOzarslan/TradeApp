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
                    .Fields(Field.Symbol, Field.RegularMarketPrice, Field.RegularMarketChangePercent)
                    .QueryAsync();

                foreach (var symbol in request.Symbol)
                {
                    if (securities.TryGetValue(symbol, out var data))
                    {
                        stocks.Add(new FreeStockResponseList
                        {
                            Symbol = symbol,
                            Price = (decimal)(data[Field.RegularMarketPrice] ?? 0m),
                            ChangePercent = (decimal)(data[Field.RegularMarketChangePercent] ?? 0m)
                        });
                    }

                    //if (securities.TryGetValue(request.Symbol, out var data))
                    //{
                    //    return new FreeStockResponseList
                    //    {
                    //        Symbol = request.Symbol,
                    //        Price = data[Field.RegularMarketPrice],
                    //        ChangePercent = data[Field.RegularMarketChangePercent]
                    //    };
                    //}

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
