using Domain.Entities;
using Mediator;

namespace Domain.Notification;

public record DamageRecievedNotification(GameRoom GameRoom, int DamageAmount, Guid PlayerId) : INotification;
