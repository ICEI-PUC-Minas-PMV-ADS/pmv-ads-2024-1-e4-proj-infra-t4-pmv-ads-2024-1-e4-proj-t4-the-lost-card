using Domain.Entities;
using Mediator;
using System.Linq.Expressions;

namespace Domain.Notifications;

public record PlayerStatusUpdatedNotification(
    GameRoom GameRoom, 
    GameRoom.RoomGameInfo.PlayerGameInfo PlayerGameInfo, 
    Expression<Func<GameRoom.RoomGameInfo.PlayerGameInfo, int>> ValueAccessor, 
    int FreshValue
) : INotification;
