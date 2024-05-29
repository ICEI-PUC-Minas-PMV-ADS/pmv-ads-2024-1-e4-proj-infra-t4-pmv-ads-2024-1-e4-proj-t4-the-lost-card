using Application.Services;
using Domain.GameObjects.Oponents;
using Domain.Notifications;
using Mediator;

namespace Application.UseCases.GameRooms.GameEvents;

public class OponentSpawnedNotificationHandler : INotificationHandler<OponentSpawnedNotification>
{
    private readonly IGameRoomHubService gameRoomHubService;

    public OponentSpawnedNotificationHandler(IGameRoomHubService gameRoomHubService)
    {
        this.gameRoomHubService = gameRoomHubService;
    }

    public async ValueTask Handle(OponentSpawnedNotification notification, CancellationToken cancellationToken)
    {
        var dispatch = new OponentSpawnedNotificationDispatch(
            notification.RoomEncounterInfo.OponentGameId,
            notification.RoomEncounterInfo.OponentMaxLife,
            notification.RoomEncounterInfo.OponentLife,
            notification.RoomEncounterInfo.OponentIntent
        );

        await gameRoomHubService.Dispatch(dispatch, cancellationToken);
    }
}

public record OponentSpawnedNotificationDispatch(long GameId, int MaxLife, int CurrentLife, OponentIntent Intent);
