using Application.Contracts.LostCardDatabase;
using Application.UseCases.GameRooms.ServerTick;
using Domain.GameObjects.GameClasses;
using FluentResults;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms.GameActions.ChooseClass;

public record ChooseClassGameActionRequest(long GameClassId) : GameRoomHubRequest<ChooseClassGameActionRequestResponse>, ITurnEndingGameRoomActionRequest
{
    [JsonIgnore]
    public override bool RequiresAuthorization => true;
}

public record ChooseClassGameActionRequestResponse(long GameClassId, string Name) : GameRoomHubRequestResponse { }

public class ChooseClassGameActionRequestHandler : IGameRoomRequestHandler<ChooseClassGameActionRequest, ChooseClassGameActionRequestResponse>
{
    private readonly IGameRoomRepository gameRoomRepository;

    public ChooseClassGameActionRequestHandler(IGameRoomRepository gameRoomRepository)
    {
        this.gameRoomRepository = gameRoomRepository;
    }

    public async ValueTask<Result<GameRoomHubRequestResponse>> Handle(ChooseClassGameActionRequest request, CancellationToken cancellationToken)
    {
        if (request.Requester is null)
            return new GameRoomHubRequestError<ChooseClassGameActionRequestResponse>("Requester not found");

        if (request.CurrentRoom is null)
            return new GameRoomHubRequestError<ChooseClassGameActionRequestResponse>("Requester is not in a room");

        var gameClassId = request.GameClassId;

        if (!GameClasses.Dictionary.TryGetValue(gameClassId, out var choosenClass))
            return new GameRoomHubRequestError<ChooseClassGameActionRequestResponse>("Selected class does not exist");

        if (request.RequesterPlayerInfo is null)
            return new GameRoomHubRequestError<ChooseClassGameActionRequestResponse>("Room has no record of player joining");

        var requesterPlayerInfo = request.RequesterPlayerInfo;

        requesterPlayerInfo.GameClassId = gameClassId;
        requesterPlayerInfo.Cards = choosenClass.StartingHand.ToHashSet();
        requesterPlayerInfo.ActionsFinished = true;
        requesterPlayerInfo.CurrentBlock = 0;
        requesterPlayerInfo.MaxLife = requesterPlayerInfo.Life = choosenClass.StartingLife;

        await gameRoomRepository.Update(request.CurrentRoom, cancellationToken);

        return new ChooseClassGameActionRequestResponse(gameClassId, request.Requester.Name);
    }
}
