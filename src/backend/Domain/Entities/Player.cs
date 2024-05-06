namespace Domain.Entities;

public partial class Player : Entity
{
    protected override string ProtectedPartitionKey { get => Id!.Value.ToString(); set => base.ProtectedPartitionKey = value; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public Guid? CurrentRoom { get; set; }
    public decimal Progrees { get; set; }
    public HashSet<AchievmentInfo> Achivements { get; set; } = new HashSet<AchievmentInfo>();
}
