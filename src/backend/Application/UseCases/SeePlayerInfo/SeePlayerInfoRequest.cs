using Application.Contracts.LostCardDatabase;
using Application.FluentResultExtensions;
using Application.Services;
using Domain.Entities;
using Domain.GameObjects;
using FluentResults;
using Mediator;
using Newtonsoft.Json;

namespace Application.UseCases.SeePlayerInfoRequest;

public sealed record SeePlayerInfoRequest(Guid PlayerGuid) : IRequest<Result<SeePlayerInfoRequestResponse>>, IRequestMetadata
{
    [JsonIgnore]
    public IRequestMetadata.Metadata? RequestMetadata { get; set; }

    [JsonIgnore]
    public bool RequiresAuthorization => true;

    [JsonIgnore]
    public Player? Requester { get; set; }

    [JsonIgnore]
    public GameRoom? CurrentRoom { get; set; }
}

public class SeePlayerInfoRequestHandler : IRequestHandler<SeePlayerInfoRequest, Result<SeePlayerInfoRequestResponse>>
{
    private readonly IPlayerRepository playerRepository;
    public SeePlayerInfoRequestHandler(IPlayerRepository playerRepository)
    {
        this.playerRepository = playerRepository;
    }

    public async ValueTask<Result<SeePlayerInfoRequestResponse>> Handle(SeePlayerInfoRequest request, CancellationToken cancellationToken)
    {

        var player = await playerRepository.Find(request.PlayerGuid, cancellationToken);

        if (player is null)
            return new ApplicationError("Player not found");

        var unlockAchievments = player.Achivements.Select(ua => Achievements.Dictionary[ua.AchievmentKey]);

        return new SeePlayerInfoRequestResponse(player.Name, player.Progrees, unlockAchievments);
    }
}

public sealed record SeePlayerInfoRequestResponse(string PlayerName, decimal Progress, IEnumerable<Achievements.Achievment> UnlockedAchievments);