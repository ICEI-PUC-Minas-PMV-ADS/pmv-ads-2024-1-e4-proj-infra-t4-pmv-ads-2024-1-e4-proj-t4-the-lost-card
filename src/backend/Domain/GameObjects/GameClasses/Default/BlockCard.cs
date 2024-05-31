using Domain.Entities;

namespace Domain.GameObjects.GameClasses.Default;

public class BlockCard : Card
{
    public int BlockValue { get; } = 5;
    public override long? GameClassId => DefaultGameClass.Value.Id;
    public override long Id { get; } = IdAssignHelper.CalculateIdHash(nameof(BlockCard));
    public override string Name => "Carta de bloqueio";
    public override string Description => $"Aumenta bloqueio em {BlockValue} pontos";

    public override void OnPlay(GameRoom gameRoom, GameRoom.RoomGameInfo.PlayerGameInfo playerGameInfo)
    {
        if (gameRoom.GameInfo!.PlayersInfo.TryGetValue(playerGameInfo, out var actualPlayerGameInfo))
            actualPlayerGameInfo.CurrentBlock += BlockValue;

        base.OnPlay(gameRoom, playerGameInfo);
    }
}
