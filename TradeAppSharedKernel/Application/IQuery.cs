using MediatR;

namespace TradeAppSharedKernel.Application
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
