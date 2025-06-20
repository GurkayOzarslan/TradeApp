namespace TradeAppEntity
{
    public class UserSignals
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int SymbolId { get; private set; }
        public Users User { get; private set; }
        public Symbols Symbol { get; private set; }

        private UserSignals() { }

        public UserSignals(int userId, int UserSignalSymbolId)
        {
            UserId = userId;
            SymbolId = UserSignalSymbolId;
        }
    }
}
