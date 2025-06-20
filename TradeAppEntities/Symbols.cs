namespace TradeAppEntity
{
    public class Symbols
    {
        public int SymbolId { get; private set; }
        public string Symbol { get; private set; }
        public ICollection<UserSymbols> UserSymbols { get; private set; }
        public ICollection<UserSignals> UserSignals { get; private set; }

    }
}
