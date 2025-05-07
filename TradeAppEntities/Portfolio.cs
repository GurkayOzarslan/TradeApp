namespace TradeAppEntity
{
    public class Portfolio
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string AssetName { get; private set; }
        public decimal Amount { get; private set; }
        public decimal PurchasePrice { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Users Users { get; private set; }
    }
}
