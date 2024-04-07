using Application.Contracts.LostCardDatabase;
using Application.FluentResultExtensions;
using FluentResults;
using FluentValidation;
using Mediator;

namespace Application.UseCases.GetPlayerAchievements
{
    public sealed record GetPlayerAchievementsRequest(string id) : IRequest<Result<GetPlayerAchievementsResponse>>  
    {
        public class Validator : AbstractValidator<GetPlayerAchievementsRequest>
        {
            public Validator()
            {
                RuleFor(x => x.id).NotEmpty().NotNull();
            }
        }
    }

    public class GetPlayerAchievementsRequestHandler : IRequestHandler<GetPlayerAchievementsRequest, Result<GetPlayerAchievementsResponse>>
    {
        private readonly IPlayerRepository playerRepository;
        public GetPlayerAchievementsRequestHandler(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public async ValueTask<Result<GetPlayerAchievementsResponse>> Handle(GetPlayerAchievementsRequest request, CancellationToken cancellationToken)
        {
            var player = await playerRepository.Find(request.id,cancellationToken);

            if (player is null)
                return new ApplicationError("Player not found");

            var achievements = player.Achivements.Select(a=> new PlayerAchievements(a.Name,a.Descriptions)).ToList();

            return new GetPlayerAchievementsResponse(player.Name,player.Progrees,achievements);
        }
    }
    public sealed record GetPlayerAchievementsResponse(string PlayerName,decimal Progress,IEnumerable<PlayerAchievements> AchievementsName);

    public sealed record PlayerAchievements(string AchievementName, string AchievementDescription);

}
