using Application.FluentResultExtensions;
using Application.Services;
using Application.UseCases.GameRooms;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Application.Behaviours;

public class ExceptionHandlingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    private readonly ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> logger;
    private readonly IRequestMetadataService requestMetadataService;

    public ExceptionHandlingBehaviour(ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> logger, IRequestMetadataService requestMetadataService)
    {
        this.logger = logger;
        this.requestMetadataService = requestMetadataService;
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

            logger.LogError(errorMessage);

            // Azure log currently requires exceptions to be logged like this
            logger.LogError("{Message}", e.Message);
            logger.LogError("{StackTrace}", e.StackTrace);

            var error = requestMetadataService.IsHubRequest ? new GameRoomHubRequestError<GameRoomHubRequestResponse>(errorMessage) : new Error(errorMessage);

            return Result.Fail(error).To<TResponse>();
        }
    }
}
