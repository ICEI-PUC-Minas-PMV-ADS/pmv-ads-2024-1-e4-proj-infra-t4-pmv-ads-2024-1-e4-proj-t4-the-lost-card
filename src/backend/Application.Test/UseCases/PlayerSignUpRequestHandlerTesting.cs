using Application.Contracts.LostCardDatabase;
using Application.Services;
using Application.UseCases.PlayerSignUp;
using Domain.Entities;
using FluentResults;
using NSubstitute;

namespace Application.Test.UseCases;

public class PlayerSignUpRequestHandlerTesting
{
    private readonly IPlayerRepository playerRepository = Substitute.For<IPlayerRepository>();
    private readonly ICryptographyService cryptographyService = Substitute.For<ICryptographyService>();
    private readonly ILostCardDbUnitOfWork lostCardDbUnitOfWork = Substitute.For<ILostCardDbUnitOfWork>();

    private readonly PlayerSignUpRequestHandler sut;

    public PlayerSignUpRequestHandlerTesting()
    {
        sut = new(cryptographyService, playerRepository, lostCardDbUnitOfWork);
    }

    [Fact]
    public async Task WhenPlayerCanSignUp_ReturnSuccessResult()
    {
        var name = "Tester";
        var password = "Test@12345";
        var email = "test@test.com";

        var request = new PlayerSignUpRequest(name, email, password);

        var result = await sut.Handle(request, CancellationToken.None);

        cryptographyService.Received().GenerateSaltedSHA512Hash(password);

        await playerRepository.Received().Create(Arg.Any<Player>(), CancellationToken.None);
        await lostCardDbUnitOfWork.Received().SaveChangesAsync(CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task WhenPlayerIsAlreadyRegistred_ReturnResultWithFail()
    {
        var name = "Tester";
        var password = "Test@12345";
        var email = "test@test.com";

        var alredyRegistredPlayer = new Player();
        playerRepository.Find(email).Returns(alredyRegistredPlayer);

        var request = new PlayerSignUpRequest(name, email, password);

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.IsType<Error>(result.Errors[0]);
    }
}
