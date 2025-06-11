using System.Security.Claims;

namespace TradeAppSharedKernel.Infrastructure.Helpers.TokenInfo
{
    public interface ITokenInfoHandler
    {
        TokenInfoResponse TokenInfo();
    }
}
