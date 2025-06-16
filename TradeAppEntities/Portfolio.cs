namespace TradeAppEntity
{
    public class Portfolio
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string Symbol { get; private set; }
        public decimal Amount { get; private set; }
        public decimal PurchasePrice { get; private set; }

        public Users Users { get; private set; }
    }
}
