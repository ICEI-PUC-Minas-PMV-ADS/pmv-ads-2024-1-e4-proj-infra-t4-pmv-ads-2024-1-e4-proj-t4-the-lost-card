using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.GameObjects;
using Domain.GameObjects.Oponents;
using Domain.Notifications;
using Mediator;

namespace Application.UseCases.GameRooms.GameEvents;

public record TurnStartedNotifcationDispatch(bool IsNewLevel);

public class TurnStartedNotifcationHandler : INotificationHandler<TurnStartedNotifcation>
{
    private readonly IGameRoomRepository gameRoomRepository;
    private readonly IGameRoomHubService gameRoomHubService;

    public TurnStartedNotifcationHandler(IGameRoomRepository gameRoomRepository, IGameRoomHubService gameRoomHubService)
    {
        this.gameRoomRepository = gameRoomRepository;
        this.gameRoomHubService = gameRoomHubService;
    }

    public async ValueTask Handle(TurnStartedNotifcation notification, CancellationToken cancellationToken)
    {
        var isNewLevel = notification.GameRoom.GameInfo!.EncounterInfo!.Oponent!.CurrentLife <= 0;

        if (isNewLevel)
        {
            notification.GameRoom.GameInfo!.CurrentLevel++;

            var randomOponent = Oponents.All.Where(o => o.MinLevel <= notification.GameRoom.GameInfo!.CurrentLevel && o.MaxLevel > notification.GameRoom.GameInfo!.CurrentLevel).OrderBy(x => Guid.NewGuid()).First();
            randomOponent.CurrentLife = randomOponent.MaxLife;
            randomOponent.CurrentIntent = randomOponent.GetNewIntent(notification.GameRoom);
            notification.GameRoom.GameInfo!.EncounterInfo.Oponent = randomOponent;
            notification.GameRoom.GameInfo!.EncounterInfo.CurrentTurn = 0;
            gameRoomHubService.AddDelayed(new OponentSpawnedNotification(notification.GameRoom.GameInfo!.EncounterInfo));
        }
        else
        {
            notification.GameRoom.GameInfo!.EncounterInfo!.CurrentTurn++;
            var currentIntent = notification.GameRoom.GameInfo!.EncounterInfo!.Oponent!.CurrentIntent;
            var notifcations = currentIntent!.OnPlay(notification.GameRoom);
            gameRoomHubService.AddDelayed(notifcations);
            notification.GameRoom.GameInfo!.EncounterInfo!.Oponent!.CurrentIntent = notification.GameRoom.GameInfo!.EncounterInfo!.Oponent!.GetNewIntent(notification.GameRoom);
        }

        foreach (var playerGameInfo in notification.GameRoom.GameInfo!.PlayersInfo)
        {
            playerGameInfo.ActionsFinished = false;
            gameRoomHubService.AddDelayed(new PlayerStatusUpdatedNotification(notification.GameRoom, playerGameInfo, p => p.CurrentEnergy, playerGameInfo.MaxEnergy));
            gameRoomHubService.AddDelayed(new PlayerStatusUpdatedNotification(notification.GameRoom, playerGameInfo, p => p.CurrentBlock, 0));

            if (!isNewLevel)
            {
                playerGameInfo.DiscardPile = new HashSet<Card>(playerGameInfo.DiscardPile.Concat(playerGameInfo.Hand));
                var cardsDrawFromDrawPile = playerGameInfo.DrawPile.OrderBy(x => Guid.NewGuid()).Take(5);
                var cardsDrawFromDrawPileCount = cardsDrawFromDrawPile.Count();
                HashSet<Card>? cardsDrawnInTotal;

                if (cardsDrawFromDrawPileCount < 5)
                {
                    playerGameInfo.DrawPile = new HashSet<Card>(playerGameInfo.DiscardPile);
                    var cardsDrawFromDiscardPile = playerGameInfo.DrawPile.OrderBy(x => Guid.NewGuid()).Take(5 - cardsDrawFromDrawPileCount);
                    cardsDrawnInTotal = new HashSet<Card>(cardsDrawFromDrawPile.Concat(cardsDrawFromDiscardPile));
                }
                else
                {
                    cardsDrawnInTotal = new HashSet<Card>(cardsDrawFromDrawPile);
                }

                playerGameInfo.Hand = cardsDrawnInTotal;

                var connectionId = notification.GameRoom.Players.First(x => x.PlayerId == playerGameInfo.PlayerId).ConnectionId;

                var handShuffledNotification = new HandShuffledNotification(playerGameInfo.PlayerId, connectionId, playerGameInfo.PlayerName, playerGameInfo.Hand, playerGameInfo.DrawPile, playerGameInfo.DiscardPile);
                gameRoomHubService.AddDelayed(handShuffledNotification);
            }
        }

        await gameRoomRepository.Update(notification.GameRoom, cancellationToken);
        await gameRoomHubService.Dispatch(new TurnStartedNotifcationDispatch(isNewLevel), cancellationToken);
    }
}
