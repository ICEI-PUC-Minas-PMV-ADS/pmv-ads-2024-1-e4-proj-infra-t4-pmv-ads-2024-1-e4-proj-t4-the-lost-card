using Domain.Entities;
using Domain.Notifications;
using Mediator;

namespace Domain.GameObjects.Oponents.Intents;

public class AttackIntent : OponentIntent
{
    public int DamageAmount { get; set; }

    public override IntentType Type { get; } = IntentType.Agressive;

    public override long Id { get; } = IdAssignHelper.CalculateIdHash(nameof(AttackIntent));

    public override IEnumerable<INotification> OnPlay(GameRoom gameRoom)
    {
        foreach (var playerInfo in gameRoom.GameInfo!.PlayersInfo)
        {
            var blockedDamage = DamageAmount <= playerInfo.CurrentBlock ? DamageAmount : playerInfo.CurrentBlock;
            yield return new PlayerStatusUpdatedNotification(gameRoom, playerInfo, p => p.CurrentBlock, playerInfo.CurrentBlock - blockedDamage);

            yield return new PlayerStatusUpdatedNotification(gameRoom, playerInfo, p => p.CurrentLife, playerInfo.CurrentLife - (DamageAmount - blockedDamage));
        }
    }
}
