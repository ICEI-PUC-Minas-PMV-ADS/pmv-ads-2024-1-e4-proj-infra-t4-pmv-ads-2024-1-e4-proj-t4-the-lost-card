using FluentResults;
using Mediator;

namespace Application.UseCases.GameRooms.ServerTick;

public interface ITurnEndingActionRequest { };

public class GameRoomTurnEndingActionPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : GameRoomHubRequestBase, ITurnEndingActionRequest
    where TResponse : Result<GameRoomHubRequestResponse>
{
    private readonly ISender sender;

    public GameRoomTurnEndingActionPipelineBehaviour(ISender sender)
    {
        this.sender = sender;
    }

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var response = await next(message, cancellationToken);

        if (response.IsFailed) return response;

        if (message.CurrentRoom?.GameInfo?.PlayersInfo.All(x => x.ActionsFinished) is true)
            _ = await sender.Send(new ServerTickGameRoomRequest(message.CurrentRoom));

        return response;
    }
}
