using Application.Contracts.LostCardDatabase;
using Application.UseCases.GameRooms.ServerTick;
using Domain.GameObjects.GameClasses;
using FluentResults;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms.GameActions.ChooseClass;

public record ChooseClassGameActionRequest(int GameClassId) : GameRoomHubRequest<ChooseClassGameActionRequestResponse>, ITurnEndingGameRoomActionRequest
{
    [JsonIgnore]
    public override bool RequiresAuthorization => true;
}

public record ChooseClassGameActionRequestResponse(int GameClassId, string Name, string PlayerId) : GameRoomHubRequestResponse { }

public class ChooseClassGameActionRequestHandler : IGameRoomRequestHandler<ChooseClassGameActionRequest, ChooseClassGameActionRequestResponse>
{
    private readonly ILostCardDbUnitOfWork unitOfWork;

    public ChooseClassGameActionRequestHandler(ILostCardDbUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async ValueTask<Result<GameRoomHubRequestResponse>> Handle(ChooseClassGameActionRequest request, CancellationToken cancellationToken)
    {
        if (request.Requester is null)
            return new Error("Requester not found");

        if (request.CurrentRoom is null)
            return new Error("Requester is not in a room");

        var gameClassId = request.GameClassId;

        if (!GameClasses.Dictionary.TryGetValue(gameClassId, out var choosenClass))
            return new Error("Selected class does not exist");

        if (request.RequesterPlayerInfo is null)
            return new Error("Room has no record of player joining");

        var requesterPlayerInfo = request.RequesterPlayerInfo;

        requesterPlayerInfo.GameClassId = gameClassId;
        requesterPlayerInfo.Cards = choosenClass.StartingHand.ToHashSet();
        requesterPlayerInfo.ActionsFinished = true;
        requesterPlayerInfo.CurrentBlock = 0;
        requesterPlayerInfo.MaxLife = requesterPlayerInfo.Life = choosenClass.StartingLife;

        unitOfWork.GameRoomRepository.Update(request.CurrentRoom);
        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ChooseClassGameActionRequestResponse(gameClassId, request.Requester.Name, request.Requester.Id!.Value.ToString());
    }
}
