using Domain.GameObjects.Oponents;

namespace Domain.Entities;

public partial class GameRoom
{
    public partial class RoomGameInfo
    {
        public partial class RoomEncounterInfo
        {
            public Oponent? Oponent { get; set; }
            public int CurrentTurn { get; set; }
        }
    }
}
