namespace Domain.Entities;

public partial class GameRoom : Entity
{
    protected override string ProtectedPartitionKey { get => AdminId!.Value.ToString(); set => base.ProtectedPartitionKey = value; }

    public bool IsInviteOnly { get; set; }
    public string Name { get; set; } = "Public lobby";
    public Guid? AdminId { get; set; }
    public HashSet<RoomPlayerInfo> Players { get; set; } = new();
    public RoomGameInfo? GameInfo { get; set; } = new();
    public SemaphoreState Semaphore { get; set; }
}
