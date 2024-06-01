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

public record TurnStartedNotificationDispatch();

public record EndTurnGameActionRequestResponse : GameRoomHubRequestResponse;

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
        request.RequesterPlayerInfo!.CurrentEnergy = request.RequesterPlayerInfo!.MaxEnergy;

        if (request.CurrentRoom?.GameInfo?.PlayersInfo.All(x => x.ActionsFinished) is true)
        {
            if (request.CurrentRoom.GameInfo!.EncounterInfo!.Oponent!.CurrentLife <= 0)
            {
                request.CurrentRoom.GameInfo!.CurrentLevel++;

                var randomOponent = Oponents.All.Where(o => o.MinLevel <= request.CurrentRoom.GameInfo!.CurrentLevel && o.MaxLevel > request.CurrentRoom.GameInfo!.CurrentLevel).OrderBy(x => Guid.NewGuid()).First();
                randomOponent.CurrentLife = randomOponent.MaxLife;
                randomOponent.CurrentIntent = randomOponent.GetNewIntent(request.CurrentRoom);
                request.CurrentRoom.GameInfo!.EncounterInfo.Oponent = randomOponent;
                request.CurrentRoom.GameInfo!.EncounterInfo.CurrentTurn = 0;
                gameRoomHubService.AddDelayed(new OponentSpawnedNotification(request.CurrentRoom.GameInfo!.EncounterInfo));
            }
            else
            {
                var currentIntent = request.CurrentRoom.GameInfo!.EncounterInfo!.Oponent!.CurrentIntent;
                var notifcations = currentIntent!.OnPlay(request.CurrentRoom);
                gameRoomHubService.AddDelayed(notifcations);
                request.CurrentRoom.GameInfo!.EncounterInfo!.Oponent!.CurrentIntent = request.CurrentRoom.GameInfo!.EncounterInfo!.Oponent!.GetNewIntent(request.CurrentRoom);

                foreach (var playerGameInfo in request.CurrentRoom.GameInfo?.PlayersInfo as IEnumerable<GameRoom.RoomGameInfo.PlayerGameInfo> ?? Array.Empty<GameRoom.RoomGameInfo.PlayerGameInfo>())
                    playerGameInfo.ActionsFinished = false;
                request.CurrentRoom.GameInfo!.EncounterInfo!.CurrentTurn++;
            }

            await gameRoomRepository.Update(request.CurrentRoom, cancellationToken);

            await gameRoomHubService.Dispatch(new TurnStartedNotificationDispatch(), cancellationToken);
        }

        return new EndTurnGameActionRequestResponse();
    }
}
