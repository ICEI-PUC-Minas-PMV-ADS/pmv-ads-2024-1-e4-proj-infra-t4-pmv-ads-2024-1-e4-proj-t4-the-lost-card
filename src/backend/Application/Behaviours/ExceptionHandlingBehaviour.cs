using Application.FluentResultExtensions;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Application.Behaviours;

public class ExceptionHandlingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    private readonly ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> logger;

    public ExceptionHandlingBehaviour(ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        try
        {
            return await next(message, cancellationToken);
        }
        catch (Exception e)
        {
            const string errorMessage = "An unhandled exception has occured";

            logger.LogError(exception: e, errorMessage);

            return Result.Fail(new Error(errorMessage)).To<TResponse>();
        }
    }
}
