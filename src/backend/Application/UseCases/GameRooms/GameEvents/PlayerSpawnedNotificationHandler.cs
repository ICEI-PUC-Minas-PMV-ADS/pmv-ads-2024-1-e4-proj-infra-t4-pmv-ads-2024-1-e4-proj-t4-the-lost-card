using Application.Services;
using Domain.GameObjects.GameClasses;
using Domain.Notifications;
using Mediator;

namespace Application.UseCases.GameRooms.GameEvents;

public record PlayerSpawnedNotificationDispatch(string Name, int CurrentLife, int MaxLife, int CurrentBlock, long GameClassId, string GameClassName);

public class PlayerSpawnedNotificationHandler : INotificationHandler<PlayerSpawnedNotification>
{
    private readonly IGameRoomHubService gameRoomHubService;
    public PlayerSpawnedNotificationHandler(IGameRoomHubService gameRoomHubService)
    {
        this.gameRoomHubService = gameRoomHubService;
    }

    public async ValueTask Handle(PlayerSpawnedNotification notification, CancellationToken cancellationToken)
    {
        var dispatch = new PlayerSpawnedNotificationDispatch(
            notification.PlayerGameInfo.PlayerName,
            notification.PlayerGameInfo.Life,
            notification.PlayerGameInfo.MaxLife,
            notification.PlayerGameInfo.CurrentBlock,
            notification.PlayerGameInfo.GameClassId!.Value,
            GameClasses.Dictionary[notification.PlayerGameInfo.GameClassId!.Value].Name

        );

        await gameRoomHubService.Dispatch(dispatch, cancellationToken);
    }
}
