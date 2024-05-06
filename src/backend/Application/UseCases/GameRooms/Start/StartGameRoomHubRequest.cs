using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Entities;
using FluentResults;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms.Start;

public record StartGameRoomHubRequestResponse : GameRoomHubRequestResponse;

public record StartGameRoomHubRequest() : GameRoomHubRequest<StartGameRoomHubRequestResponse>, IRequestMetadata
{
    [JsonIgnore]
    public override bool RequiresAuthorization => true;
}

public class StartGameRoomRequestHandler : IGameRoomRequestHandler<StartGameRoomHubRequest, StartGameRoomHubRequestResponse>
{
    private readonly ILostCardDbUnitOfWork dbUnitOfWork;

    public StartGameRoomRequestHandler(ILostCardDbUnitOfWork dbUnitOfWork)
    {
        this.dbUnitOfWork = dbUnitOfWork;
    }

    public async ValueTask<Result<GameRoomHubRequestResponse>> Handle(StartGameRoomHubRequest request, CancellationToken cancellationToken)
    {
        if (request.Requester is null)
            return Result.Fail("Requester not found");

        if (request.CurrentRoom is null)
            return Result.Fail("Requester hasnt joined a room");

        if (request.CurrentRoom.AdminId != request.Requester.Id)
            return Result.Fail("Cant start a gameroom youre not admin of");

        if (request.CurrentRoom is not { State: GameRoomState.Lobby })
            return Result.Fail("Game room is not lobby anymore");

        if (request.CurrentRoom.Players.Count < 2)
            return Result.Fail("Gamerooms can only start after atleast two people join");

        request.CurrentRoom.GameInfo = new GameRoom.RoomGameInfo
        {
            CurrentLevel = 1,
            EncounterInfo = null,
            PlayersInfo = request.CurrentRoom.Players.Select(p => new GameRoom.RoomGameInfo.PlayerGameInfo
            {
                ActionsFinished = false,
                PlayerId = p.PlayerId,
                GameClassId = null,
                Life = int.MinValue,
                MaxLife = int.MinValue,
                CurrentBlock = int.MinValue
            }).ToHashSet()
        };

        request.CurrentRoom.State = GameRoomState.Started;

        dbUnitOfWork.GameRoomRepository.Update(request.CurrentRoom);

        _ = await dbUnitOfWork.SaveChangesAsync(cancellationToken);

        return new StartGameRoomHubRequestResponse();
    }
}
