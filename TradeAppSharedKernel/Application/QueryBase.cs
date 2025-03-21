using MediatR;

namespace TradeAppSharedKernel.Application
{
    public abstract class QueryBase<TResponse> : IRequest<TResponse>
    {
    }
}
