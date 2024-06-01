using Application.Services;
using Domain.GameObjects.GameClasses;
using Domain.Notifications;
using Mediator;

namespace Application.UseCases.GameRooms.GameEvents;

public record PlayerSpawnedNotificationDispatch(string Name, int CurrentLife, int MaxLife, int CurrentBlock, long GameClassId, string GameClassName, int CurrentEnergy, int MaxEnergy);

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
            notification.PlayerGameInfo.CurrentLife,
            notification.PlayerGameInfo.MaxLife,
            notification.PlayerGameInfo.CurrentBlock,
            notification.PlayerGameInfo.GameClassId!.Value,
            GameClasses.Dictionary[notification.PlayerGameInfo.GameClassId!.Value].Name,
            notification.PlayerGameInfo.CurrentEnergy,
            notification.PlayerGameInfo.MaxEnergy
        );

        await gameRoomHubService.Dispatch(dispatch, cancellationToken);
    }
}
