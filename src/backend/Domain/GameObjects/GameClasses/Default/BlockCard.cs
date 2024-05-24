using Domain.Entities;
using Newtonsoft.Json;

namespace Domain.GameObjects.GameClasses.Default;

public class BlockCard : Card
{
    public int BlockValue { get; } = 5;
    public override int? GameClassId => 1;
    public override long Id { get; } = IdAssignHelper.CalculateIdHash(nameof(BlockCard));
    public override string Name => "Carta de bloqueio";
    public override string Description => $"Bloqueia o usuário por {BlockValue} turnos, impossibilitando as suas jogadas.";

    public override void OnPlay(GameRoom gameRoom, GameRoom.RoomGameInfo.PlayerGameInfo playerGameInfo)
    {
        if (gameRoom.GameInfo!.PlayersInfo.TryGetValue(playerGameInfo, out var actualPlayerGameInfo))
            actualPlayerGameInfo.CurrentBlock += BlockValue;

        base.OnPlay(gameRoom, playerGameInfo);
    }
}
