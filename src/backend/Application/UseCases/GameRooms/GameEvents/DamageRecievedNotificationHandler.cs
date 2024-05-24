using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Notification;
using Mediator;

namespace Application.UseCases.GameRooms.GameEvents;

public class DamageRecievedNotificationHandler : INotificationHandler<DamageRecievedNotification>
{
    private readonly IGameRoomHubService gameRoomHubService;
    private readonly IGameRoomRepository gameRoomRepository;

    public DamageRecievedNotificationHandler(
        IGameRoomHubService gameRoomHubService, 
        IGameRoomRepository gameRoomRepository
    )
    {
        this.gameRoomHubService = gameRoomHubService;
        this.gameRoomRepository = gameRoomRepository;
    }

    public async ValueTask Handle(DamageRecievedNotification notification, CancellationToken cancellationToken)
    {
        var playerInfo = notification.GameRoom.GameInfo!.PlayersInfo.First(pi => pi.PlayerId == notification.PlayerId);

        var defenseDamageExcess = playerInfo.CurrentBlock - notification.DamageAmount;

        var blockedDamage = Math.Max(playerInfo.CurrentBlock - notification.DamageAmount, playerInfo.CurrentBlock);
        var lifeDamage = defenseDamageExcess < 0 ? notification.DamageAmount - blockedDamage : 0;

        playerInfo.CurrentBlock = Math.Max(playerInfo.CurrentBlock - blockedDamage, 0);
        playerInfo.Life = Math.Max(playerInfo.Life - blockedDamage, 0);

        await gameRoomRepository.Update(notification.GameRoom, cancellationToken);

        await gameRoomHubService.Dispatch(new DamageRecievedHubEvent(playerInfo.PlayerName, blockedDamage, lifeDamage, playerInfo.CurrentBlock, playerInfo.Life, playerInfo.Life < 0), cancellationToken);
    }
}

public record DamageRecievedHubEvent(string PlayerName, int BlockDamageAmount, int LifeDamageAmount, int UpdatedCurrentLife, int UpdatedCurrentBlock, bool WasKilled);
