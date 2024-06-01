using Domain.Entities;
using Domain.Notifications;
using Mediator;

namespace Domain.GameObjects.GameClasses.Default;

public class BlockCard : Card
{
    public int BlockValue => 5 + (UpgradeAmount > 0 ? 3 : 0);
    public override long? GameClassId { get; } = IdAssignHelper.CalculateIdHash(nameof(DefaultGameClass));
    public override long Id { get; } = IdAssignHelper.CalculateIdHash(nameof(BlockCard));
    public override string Name { get; } = "Defender";
    public override string Description { get; } = "Ganhe {BlockValue}🛡️";
    public override int EnergyCost { get; } = 1;

    protected override IEnumerable<INotification> InternalOnPlay(GameRoom gameRoom, GameRoom.RoomGameInfo.PlayerGameInfo playerGameInfo)
    {
        yield return new PlayerStatusUpdatedNotification(gameRoom, playerGameInfo, p => p.CurrentBlock, playerGameInfo.CurrentBlock + BlockValue);
    }
}
