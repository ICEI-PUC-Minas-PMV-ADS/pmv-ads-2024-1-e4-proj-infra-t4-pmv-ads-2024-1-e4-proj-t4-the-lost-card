using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Entities;
using FluentResults;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms.Start;

public record StartGameRoomHubRequest() : GameRoomHubRequest, IRequestMetadata
{
    [JsonIgnore]
    public IRequestMetadata.Metadata? RequestMetadata { get; set; }

    [JsonIgnore]
    public bool RequiresAuthorization => true;
}

public record StartGameRoomHubRequestResponse() : GameRoomHubRequestResponse;

public class StartGameRoomRequestHandler : IGameRoomRequestHandler<StartGameRoomHubRequest>
{
    private readonly ILostCardDbUnitOfWork dbUnitOfWork;

    public StartGameRoomRequestHandler(ILostCardDbUnitOfWork dbUnitOfWork)
    {
        this.dbUnitOfWork = dbUnitOfWork;
    }

    public async ValueTask<Result> Handle(StartGameRoomHubRequest request, CancellationToken cancellationToken)
    {
        if (request.RequestMetadata?.RequesterId is null)
            return Result.Fail("Requester not found");

        var requester = await dbUnitOfWork.PlayerRepository.Find(request.RequestMetadata.RequesterId!.Value, cancellationToken);

        if (requester is null)
            return Result.Fail("Requester not found");

        if (requester.CurrentRoom is null)
            return Result.Fail("Requester hasnt joined a room");

        var gameRoom = await dbUnitOfWork.GameRoomRepository.Find(requester.CurrentRoom!.Value, cancellationToken);

        if (gameRoom is null)
            return Result.Fail("Room not found");

        if (gameRoom.AdminId != requester.Id)
            return Result.Fail("Cant start a gameroom youre not admin of");

        if (gameRoom is not { Semaphore: GameRoom.SemaphoreState.Lobby })
            return Result.Fail("Game room is not lobby anymore");

        if (gameRoom.Players.Count < 2)
            return Result.Fail("Gamerooms can only start after atleast two people join");

        gameRoom.GameInfo = new GameRoom.RoomGameInfo
        {
            CurrentLevel = 1,
            EncounterInfo = null,
            PlayersInfo = gameRoom.Players.Select(p => new GameRoom.RoomGameInfo.PlayerGameInfo
            {
                PlayerId = p.PlayerId,
                GameClassId = null,
                Life = int.MinValue,
                MaxLife = int.MinValue,
            }).ToHashSet()
        };

        gameRoom.Semaphore = GameRoom.SemaphoreState.AwaitingPlayersActions;

        dbUnitOfWork.GameRoomRepository.Update(gameRoom);

        _ = await dbUnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
