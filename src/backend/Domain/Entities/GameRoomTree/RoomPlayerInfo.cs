namespace Domain.Entities;

public partial class GameRoom
{
    public class RoomPlayerInfo
    {
        public Guid PlayerId { get; set; }
        public string ConnectionId { get; set; } = default!;
    }
}
