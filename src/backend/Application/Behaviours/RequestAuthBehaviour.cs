using Application.Contracts.LostCardDatabase;
using Application.FluentResultExtensions;
using Application.Services;
using Application.UseCases.GameRooms.Start;
using Application.UseCases.GameRooms;
using FluentResults;
using Mediator;

namespace Application.Behaviours;

class RequestAuthBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IRequestMetadata
    where TResponse : ResultBase, new()
{
    private readonly IRequestMetadataService requestInfoService;
    private readonly IPlayerRepository playerRepository;
    private readonly IGameRoomRepository gameRoomRepository;

    public RequestAuthBehaviour(
        IRequestMetadataService requestInfoService, 
        IPlayerRepository playerRepository,
        IGameRoomRepository gameRoomRepository)
    {
        this.requestInfoService = requestInfoService;
        this.playerRepository = playerRepository;
        this.gameRoomRepository = gameRoomRepository;
    }

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        if (await requestInfoService.SetRequestMetadata(cancellationToken) is { } requestMetadata)
            message.RequestMetadata = requestMetadata;

        if (message.RequiresAuthorization && message.RequestMetadata is null)
            return Result.Fail(GetError()).To<TResponse>();

        if (message.RequestMetadata?.RequesterId is { } requesterId)
        {
            var requester = await playerRepository.Find(requesterId, cancellationToken);

            if (requester is null)
                return Result.Fail(GetError()).To<TResponse>();

            message.Requester = requester;
        }

        if (message.Requester?.CurrentRoom is { } roomId)
        {
            requestInfoService.SetRoomGuid(roomId);

            var currentRoom = await gameRoomRepository.Find(roomId, cancellationToken);

            if (currentRoom is null)
                return Result.Fail(GetError()).To<TResponse>();

            message.CurrentRoom = currentRoom;
        }

        return await next(message, cancellationToken);
    }

    private IError GetError()
    {
        return requestInfoService.IsHubRequest ? new GameRoomHubRequestError<GameRoomHubRequestResponse>(AuthError.DefaultMessage) : new AuthError();
    }
}
