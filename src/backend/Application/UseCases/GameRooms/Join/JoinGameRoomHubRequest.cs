using Application.Contracts.LostCardDatabase;
using Application.FluentResultExtensions;
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
    public IRequestMetadata.Metadata? RequestMetadata { get; set; }

    [JsonIgnore]
    public bool RequiresAuthorization => true;

    public record CreationOptionsClass(string RoomName = "Public lobby");
}

public record JoinGameRoomHubRequestResponse(string NewToken) : GameRoomHubRequestResponse;

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

        var requester = await dbUnitOfWork.PlayerRepository.Find(request.RequestMetadata.RequesterId!.Value, cancellationToken);

        if (requester is null)
            return Result.Fail("Requester not found");

        if (requester.CurrentRoom is not null)
            return Result.Fail("Requester already on a room");

        var creationOptions = request.CreationOptions ?? (request.RoomGuid is null ? new JoinGameRoomHubRequest.CreationOptionsClass() : null);

        var roomGuidResult = await EnsureRoomJoined(request, cancellationToken);

        if (roomGuidResult.IsFailed)
            return roomGuidResult.ToResult<GameRoomHubRequestResponse>();

        requester.CurrentRoom = roomGuidResult.Value;
        dbUnitOfWork.PlayerRepository.Update(requester);
        await dbUnitOfWork.SaveChangesAsync(cancellationToken);

        requestMetadataService.SetRoomGuid(roomGuidResult!.Value);

        await gameRoomHubService.JoinGroup(
            request.RequestMetadata.HubConnectionId!,
            roomGuidResult!.Value.ToString()!,
            cancellationToken
        );

        var newToken = tokenService.GetToken(requester);

        return new JoinGameRoomHubRequestResponse(newToken);
    }

    private async Task<Result<Guid>> EnsureRoomJoined(JoinGameRoomHubRequest request, CancellationToken cancellationToken = default)
    {
        if (request.RoomGuid is not null)
        {
            var existingRoom = await dbUnitOfWork.GameRoomRepository.Find(request.RoomGuid!.Value, cancellationToken);

            if (existingRoom is null)
                return Result.Fail("Room not found");

            if (existingRoom is not { Semaphore: GameRoom.SemaphoreState.Lobby})
                return Result.Fail("Cant join room that is not on lobby");

            // TODO: Adicionar verificacao de banimento e se o player ja entrou na sala
            existingRoom.Players.Add(new GameRoom.PlayerInfo(request.RequestMetadata!.RequesterId, request.RequestMetadata!.HubConnectionId!));
            dbUnitOfWork.GameRoomRepository.Update(existingRoom);

            return request.RoomGuid!.Value.ToResult();
        }

        var creationOptions = request.CreationOptions ?? new JoinGameRoomHubRequest.CreationOptionsClass();

        var newRoom = new GameRoom
        {
            AdminId = request.RequestMetadata!.RequesterId,
            Name = creationOptions.RoomName,
            Semaphore = GameRoom.SemaphoreState.Lobby,
            Players = new HashSet<GameRoom.PlayerInfo> { new(request.RequestMetadata!.RequesterId, request.RequestMetadata!.HubConnectionId!) }
        };

        await dbUnitOfWork.GameRoomRepository.Create(newRoom, cancellationToken);

        return newRoom.Id!.Value.ToResult();
    }
}