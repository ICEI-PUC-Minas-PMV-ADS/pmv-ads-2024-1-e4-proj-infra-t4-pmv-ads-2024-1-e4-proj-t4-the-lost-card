using Domain.GameObjects;
using Mediator;

namespace Domain.Notifications;

public record HandShuffledNotification(Guid PlayerId, string ConnectionId, string PlayerName, IEnumerable<Card> Hand, IEnumerable<Card> DrawPile, IEnumerable<Card> DiscardPile) : INotification;
