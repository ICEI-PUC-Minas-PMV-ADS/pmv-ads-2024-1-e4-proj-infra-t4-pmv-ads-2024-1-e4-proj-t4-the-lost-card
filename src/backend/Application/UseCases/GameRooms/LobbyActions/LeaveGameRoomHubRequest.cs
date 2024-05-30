using Application.Contracts.LostCardDatabase;
using Application.FluentResultExtensions;
using Application.Services;
using FluentResults;

namespace Application.UseCases.GameRooms.LobbyActions;

public record LeaveGameRoomHubRequest : GameRoomHubRequest<LeaveGameRoomHubResponse>, IRequestMetadata
{
}

public record LeaveGameRoomHubResponse() : GameRoomHubRequestResponse;

public class LeaveGameRoomHubRequestHandler : IGameRoomRequestHandler<LeaveGameRoomHubRequest, LeaveGameRoomHubResponse>
{
    private readonly IPlayerRepository playerRepository;
    private readonly IGameRoomRepository gameRoomRepository;
    private readonly IGameRoomHubService gameRoomHubService;

    public LeaveGameRoomHubRequestHandler(
        IPlayerRepository playerRepository,
        IGameRoomRepository gameRoomRepository,
        IGameRoomHubService gameRoomHubService
    )
    {
        this.playerRepository = playerRepository;
        this.gameRoomRepository = gameRoomRepository;
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
        await playerRepository.Update(request.Requester, cancellationToken);

        if (request.CurrentRoom is null)
            return new NotFoundError("Room not found");

        request.CurrentRoom.Players.RemoveWhere(p => p.PlayerId == request.Requester.Id);
        request.CurrentRoom.GameInfo?.PlayersInfo.RemoveWhere(p => p.PlayerId == request.Requester.Id);

        if (request.CurrentRoom.Players.Any())
            await gameRoomRepository.Update(request.CurrentRoom, cancellationToken);
        else
            await gameRoomRepository.Remove(request.CurrentRoom, cancellationToken);

        return new LeaveGameRoomHubResponse();
    }
}
