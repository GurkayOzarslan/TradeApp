namespace TradeAppEntity
{
    public class Symbols
    {
        public int SymbolId { get; set; }
        public string Symbol { get; set; }
        public ICollection<UserSymbols> UserSymbols { get; set; }

    }
}
