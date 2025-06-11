using TradeAppEntity;
using TradeAppSharedKernel.Application;

namespace TradeAppApplication.Responses.User
{
    public class UserSymbolsResponse :IMapFrom<UserSymbols>
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
    }
}
