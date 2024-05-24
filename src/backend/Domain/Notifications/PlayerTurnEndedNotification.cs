using Domain.Entities;
using Mediator;

namespace Domain.Notifications;

public record PlayerTurnEndedNotification(GameRoom GameRoom) : INotification;

