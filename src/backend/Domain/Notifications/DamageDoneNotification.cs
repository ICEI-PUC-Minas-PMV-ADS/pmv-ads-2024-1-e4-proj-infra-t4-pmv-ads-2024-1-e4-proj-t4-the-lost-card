using Domain.Entities;
using Mediator;

namespace Domain.Notifications;

public record DamageDoneNotification(GameRoom GameRoom, int DamageAmount, Guid PlayerId) : INotification;
