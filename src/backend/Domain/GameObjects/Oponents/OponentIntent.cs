using Domain.Entities;
using Mediator;

namespace Domain.GameObjects.Oponents;

public abstract class OponentIntent : GameObjectBaseClass
{
    public override string QueryKey { get; } = typeof(OponentIntent).FullName!;
    public virtual IntentType Type { get; }

    public abstract override long Id { get; }

    public enum IntentType
    {
        Unknow,
        Agressive,
        Defensive,
        AgressiveDefensive,
        Debuff,
        AgressiveDebuff,
        DefensiveDebuff,
        Buff,
        AgressiveBuff,
        DefensiveBuff,
        Stunned
    }

    public abstract IEnumerable<INotification> OnPlay(GameRoom gameRoom);
}