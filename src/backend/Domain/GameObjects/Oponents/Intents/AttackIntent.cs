using Domain.Entities;
using Domain.Notification;
using Mediator;

namespace Domain.GameObjects.Oponents.Intents;

public class AttackIntent : OponentIntent
{
    public int DamageAmount { get; set; }

    public override IntentType Type => IntentType.Agressive;

    public override long Id => IdAssignHelper.CalculateIdHash(nameof(AttackIntent));

    public override IEnumerable<INotification> OnPlay(GameRoom gameRoom)
    {
        foreach (var player in gameRoom.GameInfo!.PlayersInfo)
            yield return new DamageRecievedNotification(gameRoom, DamageAmount, player.PlayerId);
    }
}
