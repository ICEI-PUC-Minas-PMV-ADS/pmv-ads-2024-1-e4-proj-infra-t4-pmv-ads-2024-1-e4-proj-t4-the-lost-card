using Domain.Entities;
using Mediator;

namespace Domain.Notifications;

public record OponentSpawnedNotification(GameRoom.RoomGameInfo.RoomEncounterInfo RoomEncounterInfo) : INotification;
