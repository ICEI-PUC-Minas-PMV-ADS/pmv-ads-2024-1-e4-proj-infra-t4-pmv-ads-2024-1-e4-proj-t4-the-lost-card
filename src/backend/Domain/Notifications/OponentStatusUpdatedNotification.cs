using Domain.Entities;
using Domain.GameObjects.Oponents;
using Mediator;
using System.Linq.Expressions;

namespace Domain.Notifications;

public record OponentStatusUpdatedNotification(
    GameRoom GameRoom,
    Oponent Oponent,
    Expression<Func<Oponent, int>> ValueAccessor,
    int FreshValue
) : INotification;
