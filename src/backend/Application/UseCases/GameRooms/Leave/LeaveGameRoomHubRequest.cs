using Application.Contracts.LostCardDatabase;
using Application.FluentResultExtensions;
using Application.Services;
using FluentResults;

namespace Application.UseCases.GameRooms.Leave;

public record LeaveGameRoomHubRequest : GameRoomHubRequest<LeaveGameRoomHubResponse>, IRequestMetadata
{
}

public record LeaveGameRoomHubResponse() : GameRoomHubRequestResponse;

public class LeaveGameRoomHubRequestHandler : IGameRoomRequestHandler<LeaveGameRoomHubRequest, LeaveGameRoomHubResponse>
{
    private readonly ILostCardDbUnitOfWork dbUnitOfWork;
    private readonly IGameRoomHubService gameRoomHubService;

    public LeaveGameRoomHubRequestHandler(
        ILostCardDbUnitOfWork dbUnitOfWork,
        IGameRoomHubService gameRoomHubService
    )
    {
        this.dbUnitOfWork = dbUnitOfWork;
        this.gameRoomHubService = gameRoomHubService;
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

        if (request.CurrentRoom.Players.Any())
            dbUnitOfWork.GameRoomRepository.Update(request.CurrentRoom);
        else
            dbUnitOfWork.GameRoomRepository.Remove(request.CurrentRoom);

        await dbUnitOfWork.SaveChangesAsync(cancellationToken);

        return new LeaveGameRoomHubResponse();
    }
}
