namespace Domain.Entities;

public partial class GameRoom
{
    public partial class RoomGameInfo
    {
        public partial class RoomEncounterInfo
        {
            // TODO: Expandir informacoes relacionadas a monstro
            public int MonsterMaxLife { get; set; }
            public int MonsterLife { get; set; }
            public Guid MonsterGameClassId { get; set; } = default!;

            public HashSet<PlayerGameEncounterInfo> PlayersInfo { get; set; } = new();
        }
    }
}
