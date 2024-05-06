using Application.Contracts.LostCardDatabase;
using Application.FluentResultExtensions;
using Application.Services;
using FluentResults;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms.Leave;

public record LeaveGameRoomHubRequest : GameRoomHubRequest<LeaveGameRoomHubResponse>, IRequestMetadata
{
    [JsonIgnore]
    public override bool RequiresAuthorization => true;
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
        if (request.Requester is null)
            return Result.Fail("Requester not found");

        if (request.CurrentRoom is null)
            return Result.Fail("Room not found");

        await gameRoomHubService.LeaveGroup(request.RequestMetadata!.HubConnectionId!, request.CurrentRoom!.Id.ToString()!, cancellationToken);

        request.Requester.CurrentRoom = null;
        dbUnitOfWork.PlayerRepository.Update(request.Requester);
        await dbUnitOfWork.SaveChangesAsync(cancellationToken);

        if (request.CurrentRoom is null)
            return new NotFoundError("Room not found");

        request.CurrentRoom.Players.RemoveWhere(p => p.PlayerId == request.Requester.Id);

        dbUnitOfWork.GameRoomRepository.Update(request.CurrentRoom);
        await dbUnitOfWork.SaveChangesAsync(cancellationToken);

        var newToken = tokenService.GetToken(request.Requester);

        return new LeaveGameRoomHubResponse(newToken);
    }
}
