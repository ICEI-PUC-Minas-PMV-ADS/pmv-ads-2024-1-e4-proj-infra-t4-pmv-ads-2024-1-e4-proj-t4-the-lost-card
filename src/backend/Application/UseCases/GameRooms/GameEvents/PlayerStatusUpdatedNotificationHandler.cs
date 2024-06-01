using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Notifications;
using Mediator;
using System.Linq.Expressions;
using System.Reflection;

namespace Application.UseCases.GameRooms.GameEvents;

public record PlayerStatusUpdatedNotificationDispatch(string PlayerName, string StatusName, int StaleValue, int FreshValue);

public class PlayerStatusUpdatedNotificationHandler : INotificationHandler<PlayerStatusUpdatedNotification>
{
    private readonly IGameRoomRepository gameRoomRepository;
    private readonly IGameRoomHubService gameRoomHubService;

    public PlayerStatusUpdatedNotificationHandler(IGameRoomRepository gameRoomRepository, IGameRoomHubService gameRoomHubService)
    {
        this.gameRoomRepository = gameRoomRepository;
        this.gameRoomHubService = gameRoomHubService;
    }

    public async ValueTask Handle(PlayerStatusUpdatedNotification notification, CancellationToken cancellationToken)
    {
        string? propertyName = default;
        int? staleValue = default;
        if (notification.ValueAccessor.Body is MemberExpression memberSelectorExpression)
        {
            var property = memberSelectorExpression.Member as PropertyInfo;
            propertyName = property?.Name;
            staleValue = (int?)property?.GetValue(notification.PlayerGameInfo, null);
            property?.SetValue(notification.PlayerGameInfo, notification.FreshValue, null);
        }

        await gameRoomRepository.Update(notification.GameRoom, cancellationToken);

        await gameRoomHubService.Dispatch(new PlayerStatusUpdatedNotificationDispatch(notification.PlayerGameInfo.PlayerName, propertyName!, staleValue!.Value, notification.FreshValue), cancellationToken);
    }
}
