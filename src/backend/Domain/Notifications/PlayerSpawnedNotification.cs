using Domain.Entities;
using Mediator;

namespace Domain.Notifications;

public record PlayerSpawnedNotification(GameRoom.RoomGameInfo.PlayerGameInfo PlayerGameInfo) : INotification;