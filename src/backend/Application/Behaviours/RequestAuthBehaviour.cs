using Application.Contracts.LostCardDatabase;
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
    private readonly ILostCardDbUnitOfWork unitOfWork;

    public RequestAuthBehaviour(IRequestMetadataService requestInfoService, ILostCardDbUnitOfWork unitOfWork)
    {
        this.requestInfoService = requestInfoService;
        this.unitOfWork = unitOfWork;
    }

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        if (await requestInfoService.SetRequestMetadata(cancellationToken) is { } requestMetadata)
            message.RequestMetadata = requestMetadata;

        if (message.RequiresAuthorization && message.RequestMetadata is null)
            return Result.Fail(new AuthError()).To<TResponse>();

        if (message.RequestMetadata?.RequesterId is { } requesterId)
        {
            var requester = await unitOfWork.PlayerRepository.Find(requesterId, cancellationToken);

            if (requester is null)
                return Result.Fail(new AuthError()).To<TResponse>();

            message.Requester = requester;
        }

        if (message.Requester?.CurrentRoom is { } roomId)
        {
            requestInfoService.SetRoomGuid(roomId);

            var currentRoom = await unitOfWork.GameRoomRepository.Find(roomId, cancellationToken);

            if (currentRoom is null)
                return Result.Fail(new AuthError()).To<TResponse>();

            message.CurrentRoom = currentRoom;
        }

        return await next(message, cancellationToken);
    }
}
