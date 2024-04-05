using Application.Services;
using FluentResults;
using Mediator;

namespace Application.Behaviours;

class RequestInfoBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IRequestMetadata
    where TResponse : ResultBase, new()
{
    private readonly IRequestInfoService requestInfoService;

    public RequestInfoBehaviour(IRequestInfoService requestInfoService)
    {
        this.requestInfoService = requestInfoService;
    }

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        message.RequestInfo = await requestInfoService.SetRequestInfo(cancellationToken);
        return await next(message, cancellationToken);
    }
}
