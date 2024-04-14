namespace Domain.Entities;

public class GameRoom : Entity
{
    protected override string ProtectedPartitionKey { get => AdminId!.Value.ToString(); set => base.ProtectedPartitionKey = value; }

    public bool IsInviteOnly { get; set; }
    public string Name { get; set; } = "Public lobby";
    public Guid? AdminId { get; set; }
    public HashSet<PlayerInfo> Players { get; set; } = new();
    public SemaphoreState Semaphore { get; set; }

    public record PlayerInfo(Guid? PlayerId, string ConnectionId);

    public enum SemaphoreState
    {
        Lobby,
        AwaitingPlayersActions,
        AwaitingServerActions
    }
}
