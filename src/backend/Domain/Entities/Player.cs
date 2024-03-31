namespace Domain.Entities;

public class Player : Entity
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public Guid? CurrentRoom { get; set; }
    public DateTime? JoinedRoomAt { get; set; }
}
