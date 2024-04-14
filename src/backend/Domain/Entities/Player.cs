namespace Domain.Entities;

public class Player : Entity
{
    protected override string ProtectedPartitionKey { get => Id!.Value.ToString(); set => base.ProtectedPartitionKey = value; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public Guid? CurrentRoom { get; set; }
    public DateTime? JoinedRoomAt { get; set; }
    public decimal Progrees { get; set; }
    public IEnumerable<Achievements> Achivements {get;set;} = Enumerable.Empty<Achievements>();
}
