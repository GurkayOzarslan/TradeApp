namespace TradeAppSharedKernel.ExternalApiService
{
    public class StockListResponse
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal ChangePercent { get; set; }
        public decimal ChangePrice { get; set; }
    }
}
