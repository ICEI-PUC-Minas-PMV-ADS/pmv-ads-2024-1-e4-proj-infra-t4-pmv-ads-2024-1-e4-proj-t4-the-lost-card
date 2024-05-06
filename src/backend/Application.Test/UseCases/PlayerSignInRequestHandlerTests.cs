using Application.Contracts.LostCardDatabase;
using Application.FluentResultExtensions;
using Application.Services;
using Application.UseCases.PlayerSignIn;
using Domain.Entities;
using NSubstitute;
using System.Text;

namespace Application.Test.UseCases;

public class PlayerSignInRequestHandlerTests
{
    private readonly IPlayerRepository playerRepository = Substitute.For<IPlayerRepository>();
    private readonly ICryptographyService cryptographyService = Substitute.For<ICryptographyService>();
    private readonly ITokenService tokenService = Substitute.For<ITokenService>();

    private readonly PlayerSignInRequestHandler sut;

    public PlayerSignInRequestHandlerTests()
    {
        sut = new(cryptographyService, tokenService, playerRepository);
    }

    [Fact]
    public async Task WhenPlayerExistsAndPasswordIsCorrect_ReturnToken()
    {
        var name = "Tester";
        var email = "test@test.com";
        var password = "Test@12345";
        var passwodHash = Encoding.UTF8.GetBytes(password);
        var guid = Guid.NewGuid();
        var registredPlayer = new Player
        {
            Name = name,
            Id = guid,
            Email = email,
            PasswordHash = passwodHash,
            PasswordSalt = passwodHash
        };
        var returnedToken = "token";

        playerRepository.Find(email).Returns(registredPlayer);
        cryptographyService.CompareSaltedSHA512Hash(password, passwodHash, passwodHash).Returns(true);
        tokenService.GetToken(registredPlayer).Returns(returnedToken);

        var signInRequest = new PlayerSignInRequest(email, password);

        var hanldeResult = await sut.Handle(signInRequest, CancellationToken.None);

        tokenService.Received().GetToken(registredPlayer);

        Assert.True(hanldeResult.IsSuccess);
        Assert.Empty(hanldeResult.Errors);
        Assert.Equal(returnedToken, hanldeResult.Value.Token);
        Assert.Equal(guid.ToString(), hanldeResult.Value.Id);
    }

    [Fact]
    public async Task WhenPlayerExistsAndPasswordIsIncorrect_ReturnApplicationError()
    {
        var name = "Tester";
        var email = "test@test.com";
        var password = "Test@12345";
        var passwodHash = Encoding.UTF8.GetBytes(password);
        var guid = Guid.NewGuid();
        var registredPlayer = new Player
        {
            Name = name,
            Id = guid,
            Email = email,
            PasswordHash = passwodHash,
            PasswordSalt = passwodHash
        };

        playerRepository.Find(email).Returns(registredPlayer);
        cryptographyService.CompareSaltedSHA512Hash(password, passwodHash, passwodHash).Returns(false);

        var signInRequest = new PlayerSignInRequest(email, password);

        var hanldeResult = await sut.Handle(signInRequest, CancellationToken.None);

        Assert.False(hanldeResult.IsSuccess);
        Assert.NotEmpty(hanldeResult.Errors);

        var applicationError = hanldeResult.Errors.Single() as ApplicationError;
        Assert.NotNull(applicationError);
    }

    [Fact]
    public async Task WhenPlayerDoesntExists_ReturnApplicationError()
    {
        var email = "test@test.com";

        playerRepository.Find(email).Returns((Player)null!);

        var signInRequest = new PlayerSignInRequest(email, "random password");

        var hanldeResult = await sut.Handle(signInRequest, CancellationToken.None);

        Assert.False(hanldeResult.IsSuccess);
        Assert.NotEmpty(hanldeResult.Errors);

        var applicationError = hanldeResult.Errors.Single() as ApplicationError;
        Assert.NotNull(applicationError);
    }
}
