using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.GameObjects;
using FluentResults;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms.GameActions;

public record PlayCardGameActionRequest(long CardId) : GameRoomHubRequest<PlayCardGameActionRequestResponse>
{
    [JsonIgnore]
    public override bool RequiresAuthorization => true;
}

public record PlayCardGameActionRequestResponse(string PlayerName, long CardId, string CardName) : GameRoomHubRequestResponse { }

public class PlayCardGameActionRequestHandler : IGameRoomRequestHandler<PlayCardGameActionRequest, PlayCardGameActionRequestResponse>
{
    private readonly IGameRoomHubService gameRoomHubService;

    public PlayCardGameActionRequestHandler(IGameRoomHubService gameRoomHubService)
    {
        this.gameRoomHubService = gameRoomHubService;
    }

    public ValueTask<Result<GameRoomHubRequestResponse>> Handle(PlayCardGameActionRequest request, CancellationToken cancellationToken)
    {
        if (!Cards.Dictionary.TryGetValue(request.CardId, out var selectedCard))
            return ValueTask.FromResult(Result.Fail<GameRoomHubRequestResponse>(new GameRoomHubRequestError<PlayCardGameActionRequestResponse>("Card not found")));

        if (request.RequesterPlayerInfo!.CurrentLife <= 0)
            return ValueTask.FromResult(Result.Fail<GameRoomHubRequestResponse>(new GameRoomHubRequestError<PlayCardGameActionRequestResponse>("Dead players cant play cards")));

        if (request.RequesterPlayerInfo!.ActionsFinished)
            return ValueTask.FromResult(Result.Fail<GameRoomHubRequestResponse>(new GameRoomHubRequestError<PlayCardGameActionRequestResponse>("Awaiting next turn")));

        if (request.RequesterPlayerInfo!.CurrentEnergy < selectedCard.EnergyCost)
            return ValueTask.FromResult(Result.Fail<GameRoomHubRequestResponse>(new GameRoomHubRequestError<PlayCardGameActionRequestResponse>("Not enough energy")));

        var cardInHand = request.RequesterPlayerInfo?.Hand.FirstOrDefault(c => c.Id == request.CardId);

        if (cardInHand is null)
            return ValueTask.FromResult(Result.Fail<GameRoomHubRequestResponse>(new GameRoomHubRequestError<PlayCardGameActionRequestResponse>("Card not in hand")));

        request.RequesterPlayerInfo?.Hand.Remove(cardInHand);
        request.RequesterPlayerInfo?.DiscardPile.Add(cardInHand);

        var notifcations = cardInHand.OnPlay(request.CurrentRoom!, request.RequesterPlayerInfo!);

        gameRoomHubService.AddDelayed(notifcations);

        return ValueTask.FromResult(new PlayCardGameActionRequestResponse(request.RequesterPlayerInfo!.PlayerName, request.CardId, selectedCard.Name).ToResult<GameRoomHubRequestResponse>());
    }
}
