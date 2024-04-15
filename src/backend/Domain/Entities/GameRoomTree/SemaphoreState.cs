namespace Domain.Entities;

public partial class GameRoom
{
    public enum SemaphoreState
    {
        Lobby,
        AwaitingPlayersActions,
        AwaitingServerActions
    }
}
