using Domain.GameObjects;

namespace Domain.Entities;

public partial class GameRoom
{
    public partial class RoomGameInfo
    {
        public class PlayerGameInfo
        {
            public Guid PlayerId { get; set; } = default!;
            public string PlayerName { get; set; } = string.Empty;
            public long? GameClassId { get; set; }
            public HashSet<Card> Cards { get; set; } = new HashSet<Card>();
            public int MaxLife { get; set; }
            public int MaxEnergy { get; set; }
            public bool ActionsFinished { get; set; }
            public int CurrentLife { get; set; }
            public int CurrentEnergy { get; set; }
            public int CurrentBlock { get; set; }
            public HashSet<Card> Hand { get; set; } = new HashSet<Card>();
            public HashSet<Card> DrawPile { get; set; } = new HashSet<Card>();
            public HashSet<Card> DiscardPile { get; set; } = new HashSet<Card>();
        }
    }
}
