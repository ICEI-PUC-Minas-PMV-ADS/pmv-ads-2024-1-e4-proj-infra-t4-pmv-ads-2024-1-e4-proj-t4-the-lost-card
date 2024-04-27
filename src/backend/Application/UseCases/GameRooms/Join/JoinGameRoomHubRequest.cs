using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Entities;
using FluentResults;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms.Join;

public record JoinGameRoomHubRequest(
    Guid? RoomGuid = default,
    JoinGameRoomHubRequest.CreationOptionsClass? CreationOptions = default
) : GameRoomHubRequest<JoinGameRoomHubRequestResponse>, IRequestMetadata
{
    [JsonIgnore]
    public override bool RequiresAuthorization => true;

    public record CreationOptionsClass(string RoomName = "Public lobby");
}

public record JoinGameRoomHubRequestResponse(string NewToken, string Name) : GameRoomHubRequestResponse;

public class JoinGameRoomRequestHandler : IGameRoomRequestHandler<JoinGameRoomHubRequest, JoinGameRoomHubRequestResponse>
{
    private readonly ILostCardDbUnitOfWork dbUnitOfWork;
    private readonly IGameRoomHubService gameRoomHubService;
    private readonly ITokenService tokenService;
    private readonly IRequestMetadataService requestMetadataService;


    public JoinGameRoomRequestHandler(
        ILostCardDbUnitOfWork dbUnitOfWork,
        IGameRoomHubService gameRoomHubService,
        ITokenService tokenService,
        IRequestMetadataService requestMetadataService)
    {
        this.dbUnitOfWork = dbUnitOfWork;
        this.gameRoomHubService = gameRoomHubService;
        this.tokenService = tokenService;
        this.requestMetadataService = requestMetadataService;
    }

    public async ValueTask<Result<GameRoomHubRequestResponse>> Handle(JoinGameRoomHubRequest request, CancellationToken cancellationToken)
    {
        if (request.RequestMetadata?.RequesterId is null)
            return Result.Fail("Requester not found");

        if (request.Requester is null)
            return Result.Fail("Requester not found");

        if (request.CurrentRoom is not null)
            return Result.Fail("Requester already on a room");

        var creationOptions = request.CreationOptions ?? (request.RoomGuid is null ? new JoinGameRoomHubRequest.CreationOptionsClass() : null);

        var roomGuidResult = await EnsureRoomJoined(request, cancellationToken);

        if (roomGuidResult.IsFailed)
            return roomGuidResult.ToResult<GameRoomHubRequestResponse>();

        request.Requester.CurrentRoom = roomGuidResult.Value;
        dbUnitOfWork.PlayerRepository.Update(request.Requester);
        _ = await dbUnitOfWork.SaveChangesAsync(cancellationToken);

        requestMetadataService.SetRoomGuid(roomGuidResult!.Value);

        await gameRoomHubService.JoinGroup(
            request.RequestMetadata.HubConnectionId!,
            roomGuidResult!.Value.ToString()!,
            cancellationToken
        );

        var newToken = tokenService.GetToken(request.Requester);

        return new JoinGameRoomHubRequestResponse(newToken, request.Requester.Name);
    }

    private async Task<Result<Guid>> EnsureRoomJoined(JoinGameRoomHubRequest request, CancellationToken cancellationToken = default)
    {
        var playerInfo = new GameRoom.RoomPlayerInfo { PlayerId = request.RequestMetadata!.RequesterId!.Value, ConnectionId = request.RequestMetadata!.HubConnectionId! };

        if (request.RoomGuid is not null)
        {
            var existingRoom = await dbUnitOfWork.GameRoomRepository.Find(request.RoomGuid!.Value, cancellationToken);

            if (existingRoom is null)
                return Result.Fail("Room not found");

            if (existingRoom is not { State: GameRoomState.Lobby })
                return Result.Fail("Cant join room that is not on lobby");

            // TODO: Adicionar verificacao de banimento e se o player ja entrou na sala
            existingRoom.Players.Add(playerInfo);
            dbUnitOfWork.GameRoomRepository.Update(existingRoom);

            return request.RoomGuid!.Value.ToResult();
        }

        var creationOptions = request.CreationOptions ?? new JoinGameRoomHubRequest.CreationOptionsClass();

        var newRoom = new GameRoom
        {
            AdminId = request.RequestMetadata!.RequesterId,
            Name = creationOptions.RoomName,
            State = GameRoomState.Lobby,
            Players = new HashSet<GameRoom.RoomPlayerInfo> { playerInfo }
        };

        await dbUnitOfWork.GameRoomRepository.Create(newRoom, cancellationToken);

        return newRoom.Id!.Value.ToResult();
    }
}