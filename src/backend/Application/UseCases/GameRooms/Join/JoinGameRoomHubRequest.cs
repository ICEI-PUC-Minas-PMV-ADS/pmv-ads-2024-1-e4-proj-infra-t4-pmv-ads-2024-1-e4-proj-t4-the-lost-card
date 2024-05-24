using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Entities;
using Domain.GameObjects.GameClasses;
using FluentResults;

namespace Application.UseCases.GameRooms.Join;

public record JoinGameRoomHubRequest(
    Guid? RoomGuid = default,
    JoinGameRoomHubRequest.CreationOptionsClass? CreationOptions = default
) : GameRoomHubRequest<JoinGameRoomHubRequestResponse>, IRequestMetadata
{
    public record CreationOptionsClass(string RoomName = "Public lobby");
}


public record JoinGameRoomHubRequestResponse(string RoomId, string AdminName, IEnumerable<JoinGameRoomHubRequestResponse.PlayerData> Players) : GameRoomHubRequestResponse
{
    public record PlayerData(string Name, PlayerData.ClassData? Class)
    {
        public record ClassData(string Name, long Id);
    }
}

public class JoinGameRoomRequestHandler : IGameRoomRequestHandler<JoinGameRoomHubRequest, JoinGameRoomHubRequestResponse>
{
    private readonly IGameRoomHubService gameRoomHubService;
    private readonly IRequestMetadataService requestMetadataService;
    private readonly IPlayerRepository playerRepository;
    private readonly IGameRoomRepository gameRoomRepository;

    public JoinGameRoomRequestHandler(
        IGameRoomHubService gameRoomHubService,
        IRequestMetadataService requestMetadataService,
        IPlayerRepository playerRepository,
        IGameRoomRepository gameRoomRepository
    )
    {
        this.gameRoomHubService = gameRoomHubService;
        this.requestMetadataService = requestMetadataService;
        this.playerRepository = playerRepository;
        this.gameRoomRepository = gameRoomRepository;
    }

    public async ValueTask<Result<GameRoomHubRequestResponse>> Handle(JoinGameRoomHubRequest request, CancellationToken cancellationToken)
    {
        if (request.RequestMetadata?.RequesterId is null)
            return new GameRoomHubRequestError<JoinGameRoomHubRequestResponse>("Requester not found");

        if (request.Requester is null)
            return new GameRoomHubRequestError<JoinGameRoomHubRequestResponse>("Requester not found");

        if (request.CurrentRoom is not null || request.Requester.CurrentRoom is not null)
            return new GameRoomHubRequestError<JoinGameRoomHubRequestResponse>("Requester already on a room");

        var creationOptions = request.CreationOptions ?? (request.RoomGuid is null ? new JoinGameRoomHubRequest.CreationOptionsClass() : null);

        var roomResult = await EnsureRoomJoined(request, cancellationToken);

        if (roomResult.IsFailed)
            return roomResult.ToResult<GameRoomHubRequestResponse>();

        request.Requester.CurrentRoom = roomResult.Value.Id!.Value;
        await playerRepository.Update(request.Requester, cancellationToken);

        requestMetadataService.SetRoomGuid(roomResult.Value.Id!.Value);

        await gameRoomHubService.JoinGroup(
            request.RequestMetadata.HubConnectionId!,
            roomResult!.Value.Id!.Value.ToString()!,
            cancellationToken
        );

        var playerData = roomResult.Value.GameInfo!.PlayersInfo.Select(x =>
            new JoinGameRoomHubRequestResponse.PlayerData(
                x.PlayerName,
                x.GameClassId != null ? new JoinGameRoomHubRequestResponse.PlayerData.ClassData(GameClasses.Dictionary[x.GameClassId!.Value].Name, x.GameClassId!.Value) : null
            )
        );

        var playerAdminDict = roomResult.Value.GameInfo!.PlayersInfo.ToDictionary(
            x => x.PlayerName,
            x => roomResult.Value.AdminId == x.PlayerId
        );

        return new JoinGameRoomHubRequestResponse(roomResult.Value.Id!.Value.ToString(), playerAdminDict.FirstOrDefault(kvp => kvp.Value)!.Key, playerData.ToArray());
    }

    private async Task<Result<GameRoom>> EnsureRoomJoined(JoinGameRoomHubRequest request, CancellationToken cancellationToken = default)
    {
        var playerInfo = new GameRoom.RoomPlayerInfo
        {
            PlayerId = request.RequestMetadata!.RequesterId!.Value,
            PlayerName = request.Requester!.Name,
            ConnectionId = request.RequestMetadata!.HubConnectionId!
        };

        if (request.RoomGuid is not null)
        {
            var existingRoom = await gameRoomRepository.Find(request.RoomGuid!.Value, cancellationToken);

            if (existingRoom is null)
                return new GameRoomHubRequestError<JoinGameRoomHubRequestResponse>("Room not found");

            if (existingRoom is not { State: GameRoomState.Lobby })
                return new GameRoomHubRequestError<JoinGameRoomHubRequestResponse>("Cant join room that is not on lobby");

            // TODO: Adicionar verificacao de banimento e se o player ja entrou na sala
            existingRoom.Players.Add(playerInfo);

            (existingRoom.GameInfo ??= new GameRoom.RoomGameInfo()).PlayersInfo.Add(new() { PlayerId = playerInfo.PlayerId, PlayerName = request.Requester!.Name });

            await gameRoomRepository.Update(existingRoom, cancellationToken);

            return existingRoom;
        }

        var creationOptions = request.CreationOptions ?? new JoinGameRoomHubRequest.CreationOptionsClass();

        var newRoom = new GameRoom
        {
            AdminId = request.RequestMetadata!.RequesterId,
            Name = creationOptions.RoomName,
            State = GameRoomState.Lobby,
            Players = new HashSet<GameRoom.RoomPlayerInfo> { playerInfo },
            GameInfo = new GameRoom.RoomGameInfo { PlayersInfo = new() { new() { PlayerId = playerInfo.PlayerId, PlayerName = request.Requester!.Name } } }
        };

        await gameRoomRepository.Create(newRoom, cancellationToken);

        return newRoom;
    }
}