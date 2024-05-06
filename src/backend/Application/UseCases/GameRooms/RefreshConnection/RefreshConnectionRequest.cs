using Application.Contracts.LostCardDatabase;
using Application.Services;
using FluentResults;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms.RefreshConnection;

public record RefreshConnectionRequest() : GameRoomHubRequest<RefreshConnectionRequestResponse>
{
    [JsonIgnore]
    public override bool RequiresAuthorization => true;
}

public record RefreshConnectionRequestResponse : GameRoomHubRequestResponse { }

public class RefreshConnectionRequestHandler : IGameRoomRequestHandler<RefreshConnectionRequest, RefreshConnectionRequestResponse>
{
    private readonly ILostCardDbUnitOfWork unitOfWork;
    private readonly IGameRoomHubService gameRoomHubService;

    public RefreshConnectionRequestHandler(
        ILostCardDbUnitOfWork unitOfWork,
        IGameRoomHubService gameRoomHubService
    )
    {
        this.unitOfWork = unitOfWork;
        this.gameRoomHubService = gameRoomHubService;
    }

    public async ValueTask<Result<GameRoomHubRequestResponse>> Handle(RefreshConnectionRequest request, CancellationToken cancellationToken)
    {
        if (request.Requester is null)
            return new Error("Requester not found");

        if (request.CurrentRoom is null)
            return new Error("Room not found");

        if (request.RequestMetadata?.HubConnectionId is null)
            return new Error("New connection not estabileshed");

        var roomGuid = request.CurrentRoom.Id!.Value.ToString();
        var stalePalyerInfo = request.CurrentRoom.Players.FirstOrDefault(pi => pi.PlayerId == request.Requester.Id);
        await gameRoomHubService.LeaveGroup(
            stalePalyerInfo!.ConnectionId!,
            roomGuid,
            cancellationToken
        );

        var newConnectionId = request.RequestMetadata!.HubConnectionId;
        stalePalyerInfo.ConnectionId = newConnectionId;
        unitOfWork.GameRoomRepository.Update(request.CurrentRoom);
        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        await gameRoomHubService.JoinGroup(
            newConnectionId,
            roomGuid,
            cancellationToken
        );

        return new RefreshConnectionRequestResponse();
    }
}
