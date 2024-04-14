using Application.Contracts.LostCardDatabase;
using Application.FluentResultExtensions;
using Application.Services;
using FluentResults;

namespace Application.UseCases.GameRooms.Leave;

public record LeaveGameRoomHubRequest : GameRoomHubRequest<LeaveGameRoomHubResponse>, IRequestMetadata
{
    public string Discriminator => nameof(LeaveGameRoomHubRequest);

    public IRequestMetadata.Metadata? RequestMetadata { get; set; }

    public bool RequiresAuthorization => true;
}

public record LeaveGameRoomHubResponse(string NewToken) : GameRoomHubRequestResponse
{
    public string Discriminator => nameof(LeaveGameRoomHubResponse);
}

public class LeaveGameRoomHubRequestHandler : IGameRoomRequestHandler<LeaveGameRoomHubRequest, LeaveGameRoomHubResponse>
{
    private readonly ILostCardDbUnitOfWork dbUnitOfWork;
    private readonly IGameRoomHubService gameRoomHubService;
    private readonly ITokenService tokenService;

    public LeaveGameRoomHubRequestHandler(
        ILostCardDbUnitOfWork dbUnitOfWork,
        IGameRoomHubService gameRoomHubService,
        ITokenService tokenService
    )
    {
        this.dbUnitOfWork = dbUnitOfWork;
        this.gameRoomHubService = gameRoomHubService;
        this.tokenService = tokenService;
    }

    public async ValueTask<Result<GameRoomHubRequestResponse>> Handle(LeaveGameRoomHubRequest request, CancellationToken cancellationToken)
    {
        if (request.RequestMetadata?.RequesterId is null)
            return Result.Fail("Requester not found");

        var requester = await dbUnitOfWork.PlayerRepository.Find(request.RequestMetadata.RequesterId!.Value, cancellationToken);

        if (requester is null)
            return Result.Fail("Requester not found");

        var room = await dbUnitOfWork.GameRoomRepository.Find(requester.CurrentRoom!.Value, cancellationToken);

        await gameRoomHubService.LeaveGroup(request.RequestMetadata!.HubConnectionId!, requester.CurrentRoom!.ToString()!, cancellationToken);

        requester.CurrentRoom = null;
        dbUnitOfWork.PlayerRepository.Update(requester);
        await dbUnitOfWork.SaveChangesAsync(cancellationToken);

        if (room is null)
            return new NotFoundError("Room not found");

        room.Players.RemoveWhere(p => p.PlayerId == requester.Id);

        dbUnitOfWork.GameRoomRepository.Update(room);
        await dbUnitOfWork.SaveChangesAsync(cancellationToken);

        var newToken = tokenService.GetToken(requester);

        return new LeaveGameRoomHubResponse(newToken);
    }
}
