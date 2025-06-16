using TradeAppEntity;
using TradeAppSharedKernel.Application;

namespace TradeAppApplication.Responses.Stock
{
    public class GetSymbolListResponse : IMapFrom<Symbols>
    {
        public int SymbolId { get; set; }
        public string Symbol { get; set; }
    }
}
