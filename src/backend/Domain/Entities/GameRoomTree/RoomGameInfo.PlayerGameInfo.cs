using Domain.GameObjects;

namespace Domain.Entities;

public partial class GameRoom
{
    public partial class RoomGameInfo
    {
        public class PlayerGameInfo
        {
            public bool ActionsFinished { get; set; }
            public Guid PlayerId { get; set; } = default!;
            public string PlayerName { get; set; } = string.Empty;
            public long? GameClassId { get; set; }
            public int MaxLife { get; set; }
            public int Life { get; set; }
            public int CurrentBlock {  get; set; }
            public HashSet<Card> Cards { get; set; } = new HashSet<Card>();
        }
    }
}
