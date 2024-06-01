using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Notifications;
using Mediator;
using System.Linq.Expressions;
using System.Reflection;

namespace Application.UseCases.GameRooms.GameEvents;

public record OponentStatusUpdatedNotificationDispatch(string StatusName, int StaleValue, int FreshValue);

public class OponentStatusUpdatedNotificationHandler : INotificationHandler<OponentStatusUpdatedNotification>
{
    private readonly IGameRoomRepository gameRoomRepository;
    private readonly IGameRoomHubService gameRoomHubService;

    public OponentStatusUpdatedNotificationHandler(IGameRoomRepository gameRoomRepository, IGameRoomHubService gameRoomHubService)
    {
        this.gameRoomRepository = gameRoomRepository;
        this.gameRoomHubService = gameRoomHubService;
    }

    public async ValueTask Handle(OponentStatusUpdatedNotification notification, CancellationToken cancellationToken)
    {
        string? propertyName = default;
        int? staleValue = default;
        if (notification.ValueAccessor.Body is MemberExpression memberSelectorExpression)
        {
            var property = memberSelectorExpression.Member as PropertyInfo;
            propertyName = property?.Name;
            staleValue = (int?)property?.GetValue(notification.Oponent, null);
            property?.SetValue(notification.Oponent, notification.FreshValue, null);
        }

        await gameRoomRepository.Update(notification.GameRoom, cancellationToken);

        await gameRoomHubService.Dispatch(new OponentStatusUpdatedNotificationDispatch(propertyName!, staleValue!.Value, notification.FreshValue), cancellationToken);

    }
}
