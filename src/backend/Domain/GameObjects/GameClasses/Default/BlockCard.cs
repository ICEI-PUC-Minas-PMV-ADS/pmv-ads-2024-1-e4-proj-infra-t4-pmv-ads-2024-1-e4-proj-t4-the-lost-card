using Domain.Entities;

namespace Domain.GameObjects.GameClasses.Default;

public class BlockCard : Card
{
    public int BlockValue { get; } = 5;
    public override int? GameClassId => 1;
    public override int Id { get; } = GlobalCounter.Instance++;

    public override void OnPlay(GameRoom gameRoom, GameRoom.RoomGameInfo.PlayerGameInfo playerGameInfo)
    {
        if (gameRoom.GameInfo!.PlayersInfo.TryGetValue(playerGameInfo, out var actualPlayerGameInfo))
            actualPlayerGameInfo.CurrentBlock += BlockValue;

        base.OnPlay(gameRoom, playerGameInfo);
    }
}
