using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Entities;
using Domain.GameObjects.Oponents;
using Domain.Notifications;
using FluentResults;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms.GameActions;

public record EndTurnGameActionRequest() : GameRoomHubRequest<EndTurnGameActionRequestResponse>, IRequestMetadata
{
    [JsonIgnore]
    public override bool RequiresAuthorization => true;
}

public record EndTurnGameActionRequestResponse(string PlayerName) : GameRoomHubRequestResponse;

public class EndTurnGameActionRequestHandler : IGameRoomRequestHandler<EndTurnGameActionRequest, EndTurnGameActionRequestResponse>
{
    private readonly IGameRoomRepository gameRoomRepository;
    private readonly IGameRoomHubService gameRoomHubService;

    public EndTurnGameActionRequestHandler(
        IGameRoomRepository gameRoomRepository,
        IGameRoomHubService gameRoomHubService
    )
    {
        this.gameRoomRepository = gameRoomRepository;
        this.gameRoomHubService = gameRoomHubService;
    }

    public async ValueTask<Result<GameRoomHubRequestResponse>> Handle(EndTurnGameActionRequest request, CancellationToken cancellationToken)
    {
        request.RequesterPlayerInfo!.ActionsFinished = true;

        await gameRoomRepository.Update(request.CurrentRoom!, cancellationToken);

        if (request.CurrentRoom?.GameInfo?.PlayersInfo.All(x => x.ActionsFinished) is true)
            gameRoomHubService.AddDelayed(new TurnStartedNotifcation(request.CurrentRoom));

        return new EndTurnGameActionRequestResponse(request.RequesterPlayerInfo!.PlayerName);
    }
}
