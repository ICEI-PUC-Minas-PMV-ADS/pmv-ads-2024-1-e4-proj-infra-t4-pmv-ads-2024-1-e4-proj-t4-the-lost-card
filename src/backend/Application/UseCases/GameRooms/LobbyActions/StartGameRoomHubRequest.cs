using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Entities;
using Domain.GameObjects;
using Domain.GameObjects.GameClasses;
using Domain.GameObjects.Oponents;
using Domain.Notifications;
using FluentResults;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms.LobbyActions;

public record StartGameRoomHubRequestResponse : GameRoomHubRequestResponse;

public record StartGameRoomHubRequest() : GameRoomHubRequest<StartGameRoomHubRequestResponse>, IRequestMetadata
{
    [JsonIgnore]
    public override bool RequiresAuthorization => true;
}

public class StartGameRoomRequestHandler : IGameRoomRequestHandler<StartGameRoomHubRequest, StartGameRoomHubRequestResponse>
{
    private readonly IGameRoomRepository gameRoomRepository;
    private readonly IGameRoomHubService gameRoomHubService;

    public StartGameRoomRequestHandler(IGameRoomRepository gameRoomRepository, IGameRoomHubService gameRoomHubService)
    {
        this.gameRoomRepository = gameRoomRepository;
        this.gameRoomHubService = gameRoomHubService;
    }

    public async ValueTask<Result<GameRoomHubRequestResponse>> Handle(StartGameRoomHubRequest request, CancellationToken cancellationToken)
    {
        if (request.Requester is null)
            return new GameRoomHubRequestError<StartGameRoomHubRequestResponse>("Requester not found");

        if (request.CurrentRoom is null)
            return new GameRoomHubRequestError<StartGameRoomHubRequestResponse>("Requester hasnt joined a room");

        if (request.CurrentRoom.AdminId != request.Requester.Id)
            return new GameRoomHubRequestError<StartGameRoomHubRequestResponse>("Cant start a gameroom youre not admin of");

        if (request.CurrentRoom is not { State: GameRoomState.Lobby })
            return new GameRoomHubRequestError<StartGameRoomHubRequestResponse>("Game room is not lobby anymore");

        if (request.CurrentRoom.Players.Count < 2)
            return new GameRoomHubRequestError<StartGameRoomHubRequestResponse>("Gamerooms can only start after atleast two people join");

        if (request.CurrentRoom.Players.Count > 2)
            return new GameRoomHubRequestError<StartGameRoomHubRequestResponse>("Gamerooms can only start with two people (for now)");

        var randomOponent = Oponents.All.Where(o => o.MinLevel >= 1 && o.MaxLevel <= 1).OrderBy(x => Guid.NewGuid()).First();

        request.CurrentRoom.GameInfo = new GameRoom.RoomGameInfo
        {
            CurrentLevel = 1,
            EncounterInfo = new GameRoom.RoomGameInfo.RoomEncounterInfo
            {
                OponentGameId = randomOponent.Id,
                OponentIntent = randomOponent.GetIntent(request.CurrentRoom),
                OponentLife = randomOponent.StartingMaxLife,
                OponentMaxLife = randomOponent.StartingMaxLife,
                PlayersInfo = request.CurrentRoom.GameInfo!.PlayersInfo.Select(p =>
                {
                    var selectedClass = GameClasses.Dictionary[p.GameClassId!.Value];

                    var hand = selectedClass.StartingHand.OrderBy(x => Guid.NewGuid()).Take(5).ToHashSet();
                    var drawPile = selectedClass.AvailableCards.Where(ac => !hand.Contains(ac)).ToHashSet();
                    var discardPile = new HashSet<Card>();

                    var connectionId = request.CurrentRoom.Players.First(x => x.PlayerId == p.PlayerId).ConnectionId;

                    gameRoomHubService.AddDelayed(new PlayerSpawnedNotification(p));
                    gameRoomHubService.AddDelayed(new HandShuffledNotification(p.PlayerId, connectionId, p.PlayerName, hand, drawPile, discardPile));

                    return new GameRoom.RoomGameInfo.RoomEncounterInfo.PlayerGameEncounterInfo
                    {
                        DiscardPile = discardPile,
                        Hand = hand,
                        DrawPile = drawPile,
                        PlayerId = p.PlayerId
                    };
                }).ToHashSet()
            },
            PlayersInfo = request.CurrentRoom.GameInfo!.PlayersInfo
        };

        request.CurrentRoom.State = GameRoomState.Started;

        gameRoomHubService.AddDelayed(new OponentSpawnedNotification(request.CurrentRoom.GameInfo.EncounterInfo));

        await gameRoomRepository.Update(request.CurrentRoom, cancellationToken);

        return new StartGameRoomHubRequestResponse();
    }
}
