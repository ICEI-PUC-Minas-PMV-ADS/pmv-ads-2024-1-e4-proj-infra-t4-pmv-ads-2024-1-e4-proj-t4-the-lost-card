using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Extensions.Serialization;
using FluentResults;
using Mediator;

namespace Application.UseCases.GameRooms.Leave;

public record LeaveGameRoomHubRequest : GameRoomHubRequest<LeaveGameRoomHubResponse>, IJsonDerivedType<GameRoomHubRequestBase>, IRequestMetadata
{
    public string Discriminator => nameof(LeaveGameRoomHubRequest);

    public IRequestMetadata.Metadata RequestMetaData { get; set; } = default!;

    public bool RequiresAuthorization => throw new NotImplementedException();
}

public record LeaveGameRoomHubResponse(string NewToken) : GameRoomHubResponse, IJsonDerivedType<GameRoomHubResponse>
{
    public string Discriminator => nameof(LeaveGameRoomHubResponse);
}

public class LeaveGameRoomHubRequestHandler : IGameRoomRequestHandler<LeaveGameRoomHubRequest, LeaveGameRoomHubResponse>
{
    private readonly ILostCardDbUnitOfWork dbUnitOfWork;
    private readonly IGameRoomHubService gameRoomHubService;
    private readonly IGameRoomService gameRoomService;
    private readonly ITokenService tokenService;

    public LeaveGameRoomHubRequestHandler(
        ILostCardDbUnitOfWork dbUnitOfWork,
        IGameRoomHubService gameRoomHubService,
        IGameRoomService gameRoomService,
        ITokenService tokenService
    )
    {
        this.dbUnitOfWork = dbUnitOfWork;
        this.gameRoomHubService = gameRoomHubService;
        this.gameRoomService = gameRoomService;
        this.tokenService = tokenService;
    }

    public async Task<Result<GameRoomHubResponse>> Handle(LeaveGameRoomHubRequest request, CancellationToken cancellationToken)
    {
        if (request.RequestMetaData.RequesterId is null)
            return Result.Fail("Requester not found");

        var requester = await dbUnitOfWork.PlayerRepository.Find(request.RequestMetaData.RequesterId!.Value, cancellationToken);

        if (requester is null)
            return Result.Fail("Requester not found");

        var room = await gameRoomService.GetRoomFromCache(requester.CurrentRoom!.Value, cancellationToken);

        await gameRoomHubService.LeaveGroup(request.RequestMetaData!.HubConnectionId!, requester.CurrentRoom!.ToString()!, cancellationToken);

        requester.CurrentRoom = null;
        dbUnitOfWork.PlayerRepository.Update(requester);
        await dbUnitOfWork.SaveChangesAsync(cancellationToken);

        if (room is null)
            return Result.Fail("Room not found");

        room.Players.RemoveWhere(p => p.PlayerId == requester.Id);
        await gameRoomService.Update(room, cancellationToken);

        var newToken = tokenService.GetToken(requester);

        return new LeaveGameRoomHubResponse(newToken);
    }
}
