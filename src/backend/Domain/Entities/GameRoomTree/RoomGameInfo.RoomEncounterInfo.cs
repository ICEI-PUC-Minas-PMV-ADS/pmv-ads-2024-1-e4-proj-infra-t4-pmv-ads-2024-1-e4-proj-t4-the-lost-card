using Domain.GameObjects.Oponents;

namespace Domain.Entities;

public partial class GameRoom
{
    public partial class RoomGameInfo
    {
        public partial class RoomEncounterInfo
        {
            // TODO: Expandir informacoes relacionadas a monstro
            public int OponentMaxLife { get; set; }
            public int OponentLife { get; set; }
            public long OponentGameId { get; set; }
            public OponentIntent OponentIntent { get; set; } = default!;

            public HashSet<PlayerGameEncounterInfo> PlayersInfo { get; set; } = new();
        }
    }
}
