using MediatR;

namespace TradeAppSharedKernel.Application
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IRequest<TResponse>
    {
    }
}
