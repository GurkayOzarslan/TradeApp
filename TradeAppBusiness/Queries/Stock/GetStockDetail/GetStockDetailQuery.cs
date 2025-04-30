using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Text.Json;
using TradeAppApplication.Responses.Stock;
using TradeAppSharedKernel.Application;
using YahooFinanceApi;

namespace TradeAppApplication.Queries.Stock.GetStockDetail
{
    public class GetStockDetailQuery : IQuery<List<StockDetailView>>
    {
        public string Symbol { get; set; }
        public GetStockDetailQuery(string symbol)
        {
            Symbol = symbol;
        }
    }

    public class GetStockDetailQueryHandler : IQueryHandler<GetStockDetailQuery, List<StockDetailView>>
    {
        private readonly IAppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _alphaVantageApiKey;

        public GetStockDetailQueryHandler(IAppDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _alphaVantageApiKey = configuration["AlphaVantage:ApiKey"];
        }

        public async Task<List<StockDetailView>> Handle(GetStockDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={request.Symbol}&interval=60min&apikey={_alphaVantageApiKey}";

                var response = await httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                using var document = JsonDocument.Parse(content);

                if (!document.RootElement.TryGetProperty("Time Series (60min)", out var timeSeries))
                    throw new ApplicationException("Saatlik veri bulunamadı.");

                var result = new List<StockDetailView>();

                foreach (var property in timeSeries.EnumerateObject())
                {
                    var time = property.Name;
                    var priceString = property.Value.GetProperty("4. close").GetString();
                    var price = decimal.Parse(priceString, CultureInfo.InvariantCulture);

                    result.Add(new StockDetailView
                    {
                        Time = time,
                        Price = price
                    });
                }

                return result.OrderBy(x => x.Time).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Veri alınamadı: {ex.Message}");
            }
        }
    }
}
