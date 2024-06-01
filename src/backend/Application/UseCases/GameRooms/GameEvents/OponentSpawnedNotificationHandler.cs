using Application.Services;
using Domain.GameObjects.Oponents;
using Domain.Notifications;
using Mediator;

namespace Application.UseCases.GameRooms.GameEvents;

public record OponentSpawnedNotificationDispatch(long GameId, int MaxLife, int CurrentLife, OponentIntent Intent);

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
            notification.RoomEncounterInfo.Oponent!.Id,
            notification.RoomEncounterInfo.Oponent!.MaxLife,
            notification.RoomEncounterInfo.Oponent!.CurrentLife,
            notification.RoomEncounterInfo.Oponent!.CurrentIntent!
        );

        await gameRoomHubService.Dispatch(dispatch, cancellationToken);
    }
}
