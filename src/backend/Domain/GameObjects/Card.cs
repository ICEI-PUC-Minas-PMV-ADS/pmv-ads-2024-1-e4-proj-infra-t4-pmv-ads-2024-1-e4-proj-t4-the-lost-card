using Domain.Entities;
using Domain.GameObjects.GameClasses.Default;
using Domain.Notifications;
using Mediator;

namespace Domain.GameObjects;

public abstract class Card : GameObjectBaseClass
{
    public override string QueryKey { get; } = typeof(Card).FullName!;

    public abstract long? GameClassId { get; }
    public int UpgradeAmount { get; set; }
    public abstract string Name { get; }
    /// <summary>
    /// Supports string templating such as "This card deals {DamageAmount} when played."
    /// </summary>
    public abstract string Description { get; }
    public abstract int EnergyCost { get; }

    public IEnumerable<INotification> OnPlay(GameRoom gameRoom, GameRoom.RoomGameInfo.PlayerGameInfo playerGameInfo)
    {
        if (playerGameInfo.CurrentEnergy > EnergyCost)
        {
            var eneregyUpdatedNotification = new INotification[] { new PlayerStatusUpdatedNotification(gameRoom, playerGameInfo, p => p.CurrentEnergy, playerGameInfo.CurrentEnergy - EnergyCost) };
            var subsequentNotifications = InternalOnPlay(gameRoom, playerGameInfo);
            foreach (var n in subsequentNotifications.Concat(eneregyUpdatedNotification))
                yield return n;
        }
    }

    protected abstract IEnumerable<INotification> InternalOnPlay(GameRoom gameRoom, GameRoom.RoomGameInfo.PlayerGameInfo playerGameInfo);
}

public class Cards
{
    public static IEnumerable<Card> All { get; } =
        Enumerable.Empty<Card>()
            .Concat(DefaultGameClass.Value.AvailableCards);

    public static Dictionary<long, Card> Dictionary { get; } = All.ToDictionary(x => x.Id);
}