using Application.Services;
using FluentResults;
using MediatR;

namespace Application.Behaviours;

class RequestInfoBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>, IRequestMetadata
    where TResponse : ResultBase, new()
{
    private readonly IRequestInfoService requestInfoService;

    public RequestInfoBehaviour(IRequestInfoService requestInfoService)
    {
        this.requestInfoService = requestInfoService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        request.RequestInfo = await requestInfoService.SetRequestInfo(cancellationToken);
        return await next();
    }
}
