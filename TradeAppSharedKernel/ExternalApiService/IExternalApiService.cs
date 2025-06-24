namespace TradeAppSharedKernel.ExternalApiService
{
    public interface IExternalApiService
    {
        Task<List<StockListResponse>> GetStocksAsync(string token);

    }
}
