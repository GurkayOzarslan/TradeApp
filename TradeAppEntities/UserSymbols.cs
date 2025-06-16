using static System.Net.WebRequestMethods;

namespace TradeAppEntity
{
    public class UserSymbols
    {
            public int Id { get; private set; }
            public int UserId { get; private set; }
            public int SymbolId { get; private set; }
            public Users User { get; private set; }
            public Symbols Symbols { get; private set; }
            private UserSymbols() { }
            public UserSymbols(int userId, int symbolId)
            {
                UserId = userId;
                SymbolId = symbolId;
            }
    }
}
