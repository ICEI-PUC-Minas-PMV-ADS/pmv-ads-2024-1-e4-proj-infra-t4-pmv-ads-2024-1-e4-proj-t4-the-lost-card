using Domain.Entities;
using Domain.Notifications;
using Mediator;

namespace Domain.GameObjects.GameClasses.Default;

public class StrikeCard : Card
{
    public int AttackAmount => 5 + (UpgradeAmount > 0 ? 3 : 0);
    public override long? GameClassId { get; } = IdAssignHelper.CalculateIdHash(nameof(DefaultGameClass));
    public override long Id { get; } = IdAssignHelper.CalculateIdHash(nameof(StrikeCard));
    public override string Name { get; } = "Golpear";
    public override string Description => "Cause {AttackAmount}🗡️";
    public override int EnergyCost { get; } = 1;

    protected override IEnumerable<INotification> InternalOnPlay(GameRoom gameRoom, GameRoom.RoomGameInfo.PlayerGameInfo playerGameInfo)
    {
        var oponnet = gameRoom.GameInfo!.EncounterInfo!.Oponent!;

        var blockedDamage = AttackAmount <= oponnet.CurrentBlock ? AttackAmount : oponnet.CurrentBlock;
        yield return new OponentStatusUpdatedNotification(gameRoom, oponnet, p => p.CurrentBlock, oponnet.CurrentBlock - blockedDamage);

        yield return new OponentStatusUpdatedNotification(gameRoom, oponnet, p => p.CurrentLife, oponnet.CurrentLife - (AttackAmount - blockedDamage));
    }
}
