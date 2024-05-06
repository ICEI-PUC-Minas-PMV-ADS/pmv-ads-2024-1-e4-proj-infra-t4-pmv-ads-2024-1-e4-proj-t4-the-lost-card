using Domain.GameObjects;

namespace Domain.Entities;

public partial class GameRoom
{
    public partial class RoomGameInfo
    {
        public partial class RoomEncounterInfo
        {
            public class PlayerGameEncounterInfo
            {
                public Guid PlayerId { get; set; } = default!;
                public HashSet<Card> Hand { get; set; } = new HashSet<Card>();
                public HashSet<Card> DrawPile { get; set; } = new HashSet<Card>();
                public HashSet<Card> DiscardPile { get; set; } = new HashSet<Card>();
            }
        }
    }
}
