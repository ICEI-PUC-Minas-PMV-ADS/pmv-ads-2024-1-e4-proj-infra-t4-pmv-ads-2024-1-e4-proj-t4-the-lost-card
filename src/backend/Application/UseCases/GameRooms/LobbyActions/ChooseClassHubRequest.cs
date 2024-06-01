using Application.Contracts.LostCardDatabase;
using Domain.GameObjects.GameClasses;
using FluentResults;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms.LobbyActions;

public record ChooseClassHubRequest(long GameClassId) : GameRoomHubRequest<ChooseClassHubRequestResponse>
{
    [JsonIgnore]
    public override bool RequiresAuthorization => true;
}

public record ChooseClassHubRequestResponse(long GameClassId, string GameClassName, string Name) : GameRoomHubRequestResponse { }

public class ChooseClassGameActionRequestHandler : IGameRoomRequestHandler<ChooseClassHubRequest, ChooseClassHubRequestResponse>
{
    private readonly IGameRoomRepository gameRoomRepository;

    public ChooseClassGameActionRequestHandler(IGameRoomRepository gameRoomRepository)
    {
        this.gameRoomRepository = gameRoomRepository;
    }

    public async ValueTask<Result<GameRoomHubRequestResponse>> Handle(ChooseClassHubRequest request, CancellationToken cancellationToken)
    {
        if (request.Requester is null)
            return new GameRoomHubRequestError<ChooseClassHubRequestResponse>("Requester not found");

        if (request.CurrentRoom is null)
            return new GameRoomHubRequestError<ChooseClassHubRequestResponse>("Requester is not in a room");

        var gameClassId = request.GameClassId;

        if (!GameClasses.Dictionary.TryGetValue(gameClassId, out var choosenClass))
            return new GameRoomHubRequestError<ChooseClassHubRequestResponse>("Selected class does not exist");

        if (request.RequesterPlayerInfo is null)
            return new GameRoomHubRequestError<ChooseClassHubRequestResponse>("Room has no record of player joining");

        var requesterPlayerInfo = request.RequesterPlayerInfo;

        requesterPlayerInfo.GameClassId = gameClassId;
        requesterPlayerInfo.Cards = choosenClass.StartingHand.ToHashSet();
        requesterPlayerInfo.ActionsFinished = true;
        requesterPlayerInfo.CurrentBlock = 0;
        requesterPlayerInfo.MaxLife = requesterPlayerInfo.CurrentLife = choosenClass.StartingMaxLife;
        requesterPlayerInfo.CurrentEnergy = requesterPlayerInfo.MaxEnergy = choosenClass.StartingMaxEnergy;

        await gameRoomRepository.Update(request.CurrentRoom, cancellationToken);

        return new ChooseClassHubRequestResponse(gameClassId, choosenClass.Name, request.Requester.Name);
    }
}
