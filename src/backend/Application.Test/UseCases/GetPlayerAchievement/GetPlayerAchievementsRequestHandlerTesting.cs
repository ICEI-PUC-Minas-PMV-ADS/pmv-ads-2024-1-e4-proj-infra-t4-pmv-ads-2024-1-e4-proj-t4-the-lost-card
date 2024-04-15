using Application.Contracts.LostCardDatabase;
using Application.FluentResultExtensions;
using Application.Services;
using Application.UseCases.GetPlayerAchievements;
using Domain.Entities;
using NSubstitute;

namespace Application.Test.UseCases.GetPlayerAchievement;

public class GetPlayerAchievementsRequestHandlerTesting
{
    private readonly IPlayerRepository playerRepository = Substitute.For<IPlayerRepository>();
    private readonly SeePlayerInfoRequestHandler handler;
    public GetPlayerAchievementsRequestHandlerTesting()
    {
        handler = new(playerRepository);
    }
    //[Fact]
    //public async Task GetPlayerAchievements_With_Player_Sucess() 
    //{
    //    var playerId = "122bbfe2-373a-49b6-a7d0-200736fa046a";

    //    playerRepository.Find(playerId, CancellationToken.None).Returns(new Player());

    //    var request = new GetPlayerAchievementsRequest(playerId);

    //    var result = await handler.Handle(request,CancellationToken.None);

    //    Assert.True(result.IsSuccess);
    //    Assert.Empty(result.Errors);
    //}
    //[Fact]
    //public async Task GetPlayerAchievements_Without_Player_Fail() 
    //{
    //    var request = new GetPlayerAchievementsRequest(new IRequestMetadata.Metadata(new Guid(playerId), null, null, );

    //    var result = await handler.Handle(request, CancellationToken.None);

    //    Assert.False(result.IsSuccess);
    //    Assert.NotEmpty(result.Errors);
    //    Assert.IsType<ApplicationError>(result.Errors[0]);
    //}
}
