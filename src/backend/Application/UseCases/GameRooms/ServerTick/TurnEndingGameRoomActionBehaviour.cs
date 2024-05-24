using Application.Services;
using Domain.Notifications;
using FluentResults;
using Mediator;

namespace Application.UseCases.GameRooms.ServerTick;

public interface ITurnEndingGameRoomActionRequest { };

public class TurnEndingGameRoomActionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : GameRoomHubRequestBase, ITurnEndingGameRoomActionRequest
    where TResponse : Result<GameRoomHubRequestResponse>
{
    private readonly IGameRoomHubService gameRoomHubService;

    public TurnEndingGameRoomActionBehaviour(IGameRoomHubService gameRoomHubService)
    {
        this.gameRoomHubService = gameRoomHubService;
    }

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var response = await next(message, cancellationToken);

        if (response.IsFailed) return response;

        if (message.CurrentRoom?.GameInfo?.PlayersInfo.All(x => x.ActionsFinished) is true)
            gameRoomHubService.AddDelayed(new PlayerTurnEndedNotification(message.CurrentRoom));

        return response;
    }
}
