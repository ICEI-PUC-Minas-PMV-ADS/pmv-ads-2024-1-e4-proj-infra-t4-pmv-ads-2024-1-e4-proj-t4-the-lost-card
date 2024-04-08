using Application.FluentResultExtensions;
using Application.Services;
using FluentResults;
using Mediator;

namespace Application.Behaviours;

class RequestAuthBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IRequestMetadata
    where TResponse : ResultBase, new()
{
    private readonly IRequestMetadataService requestInfoService;

    public RequestAuthBehaviour(IRequestMetadataService requestInfoService)
    {
        this.requestInfoService = requestInfoService;
    }

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        if (await requestInfoService.SetRequestMetadata(cancellationToken) is { } requestMetadata)
            message.RequestMetadata = requestMetadata;
            
        if(message.RequiresAuthorization && message.RequestMetadata is null)
            return Result.Fail(new AuthError()).To<TResponse>();

        return await next(message, cancellationToken);

    }
}
