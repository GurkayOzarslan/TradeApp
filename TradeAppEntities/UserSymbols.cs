namespace TradeAppEntity
{
    public class UserSymbols
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string Symbol { get; private set; }
        public Users User { get; private set; }
        private UserSymbols() { }
        public UserSymbols(int userId, string symbol)
        {
            UserId = userId;
            Symbol = symbol;
        }
    }
}
