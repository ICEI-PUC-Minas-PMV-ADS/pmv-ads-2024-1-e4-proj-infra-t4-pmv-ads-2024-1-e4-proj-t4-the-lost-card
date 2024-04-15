using Domain.GameObjects;

namespace Domain.Entities;

public partial class GameRoom
{
    public partial class RoomGameInfo
    {
        public class PlayerGameInfo
        {
            public Guid PlayerId { get; set; } = default!;
            public Guid? GameClassId { get; set; } = default!;
            public int MaxLife { get; set; }
            public int Life { get; set; }
            public HashSet<Card> Cards { get; set; } = new HashSet<Card>();
        }
    }
}
