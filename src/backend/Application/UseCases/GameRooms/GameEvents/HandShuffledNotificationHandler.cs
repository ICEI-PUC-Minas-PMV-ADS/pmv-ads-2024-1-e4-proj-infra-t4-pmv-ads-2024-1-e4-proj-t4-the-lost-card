using Application.Services;
using Domain.GameObjects;
using Domain.Notifications;
using Mediator;

namespace Application.UseCases.GameRooms.GameEvents;

public record HandShuffledNotificationDispatch(string PlayerName, IEnumerable<Card> Hand, IEnumerable<Card> DrawPile, IEnumerable<Card> DiscardPile);

public class HandShuffledNotificationHandler : INotificationHandler<HandShuffledNotification>
{
    private readonly IGameRoomHubService gameRoomHubService;

    public HandShuffledNotificationHandler(IGameRoomHubService gameRoomHubService)
    {
        this.gameRoomHubService = gameRoomHubService;
    }
    public async ValueTask Handle(HandShuffledNotification notification, CancellationToken cancellationToken)
    {
        var (_,ConnectionId, PlayerName, Hand, DrawPile, DiscardPile) = notification;
        await gameRoomHubService.Dispatch(ConnectionId, new HandShuffledNotificationDispatch(PlayerName, Hand, DrawPile, DiscardPile), cancellationToken);
    }
}
