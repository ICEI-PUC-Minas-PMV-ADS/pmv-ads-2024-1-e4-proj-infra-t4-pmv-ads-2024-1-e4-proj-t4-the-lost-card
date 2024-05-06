using Application.Contracts.LostCardDatabase;
using Domain.Entities;
using Mediator;

namespace Application.UseCases.GameRooms.ServerTick;

public record ServerTickGameRoomRequest(GameRoom GameRoom) : IRequest;

public class ServerTickGameRoomRequestHandler : IRequestHandler<ServerTickGameRoomRequest>
{
    private readonly ILostCardDbUnitOfWork unitOfWork;

    public ServerTickGameRoomRequestHandler(ILostCardDbUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async ValueTask<Unit> Handle(ServerTickGameRoomRequest request, CancellationToken cancellationToken)
    {
        var gameRoom = request.GameRoom;

        foreach (var playerGameInfo in gameRoom.GameInfo?.PlayersInfo as IEnumerable<GameRoom.RoomGameInfo.PlayerGameInfo> ?? Array.Empty<GameRoom.RoomGameInfo.PlayerGameInfo>())
            playerGameInfo.ActionsFinished = false;

        unitOfWork.GameRoomRepository.Update(gameRoom);
        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
