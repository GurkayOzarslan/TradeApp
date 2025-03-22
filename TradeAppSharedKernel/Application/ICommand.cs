using MediatR;

namespace TradeAppSharedKernel.Application
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }

    public interface ICommand : IRequest
    {
    }
}
