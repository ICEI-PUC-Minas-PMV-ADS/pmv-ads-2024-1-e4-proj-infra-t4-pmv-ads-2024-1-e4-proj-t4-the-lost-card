using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Entities;
using Domain.GameObjects.Oponents;
using Domain.Notifications;
using Mediator;

namespace Application.UseCases.GameRooms.GameEvents;

public class ServerTickPlayerTurnEndedNotificationHandler : INotificationHandler<PlayerTurnEndedNotification>
{
    private readonly IGameRoomRepository gameRoomRepository;
    private readonly IPublisher publisher;
    private readonly IGameRoomHubService gameRoomHubService;

    public ServerTickPlayerTurnEndedNotificationHandler(
        IGameRoomRepository gameRoomRepository, 
        IPublisher publisher,
        IGameRoomHubService gameRoomHubService
    )
    {
        this.gameRoomRepository = gameRoomRepository;
        this.publisher = publisher;
        this.gameRoomHubService = gameRoomHubService;
    }

    public async ValueTask Handle(PlayerTurnEndedNotification notification, CancellationToken cancellationToken)
    {
        var gameRoom = notification.GameRoom;

        var currentIntent = gameRoom.GameInfo!.EncounterInfo!.OponentIntent;
        gameRoom.GameInfo!.EncounterInfo!.OponentIntent = Oponents.Dictionary[gameRoom.GameInfo!.EncounterInfo!.OponentGameId].GetIntent(gameRoom);
        var notifcations = currentIntent.OnPlay(gameRoom);
        await publisher.Publish(notifcations, cancellationToken);

        foreach (var playerGameInfo in gameRoom.GameInfo?.PlayersInfo as IEnumerable<GameRoom.RoomGameInfo.PlayerGameInfo> ?? Array.Empty<GameRoom.RoomGameInfo.PlayerGameInfo>())
            playerGameInfo.ActionsFinished = false;

        await gameRoomRepository.Update(gameRoom, cancellationToken);

        await gameRoomHubService.Dispatch(new TurnStartedNotificationDispatch(), cancellationToken);
    }
}

public record TurnStartedNotificationDispatch();
