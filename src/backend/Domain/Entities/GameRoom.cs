﻿namespace Domain.Entities;

public class GameRoom
{
    public Guid Guid { get; init; } = Guid.NewGuid();
    public bool IsInviteOnly { get; set; }
    public string Name { get; set; } = "Public lobby";
    public Guid? AdminId { get; set; }
    public HashSet<PlayerInfo> Players { get; set; } = new();

    public record PlayerInfo(Guid? PlayerId, string ConnectionId);
}
