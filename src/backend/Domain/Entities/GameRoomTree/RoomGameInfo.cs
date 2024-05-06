namespace Domain.Entities;

public partial class GameRoom
{
    public partial class RoomGameInfo
    {
        // TODO: Expandir niveis e mapa de encontro
        public int CurrentLevel { get; set; }
        public RoomEncounterInfo? EncounterInfo { get; set; } = default!;
        public HashSet<PlayerGameInfo> PlayersInfo { get; set; } = new();
    }
}
