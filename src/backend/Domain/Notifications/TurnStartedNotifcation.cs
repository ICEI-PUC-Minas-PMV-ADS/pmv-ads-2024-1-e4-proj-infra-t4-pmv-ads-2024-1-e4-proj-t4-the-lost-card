using Domain.Entities;
using Mediator;

namespace Domain.Notifications;

public record TurnStartedNotifcation(GameRoom GameRoom) : INotification;
