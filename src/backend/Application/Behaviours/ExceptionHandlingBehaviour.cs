using Application.FluentResultExtensions;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviours;

public class ExceptionHandlingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    private readonly ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> logger;

    public ExceptionHandlingBehaviour(ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            const string message = "An unhandled exception has occured";

            logger.LogError(exception: e, message);

            return Result.Fail(new Error(message)).To<TResponse>();
        }
    }
}
