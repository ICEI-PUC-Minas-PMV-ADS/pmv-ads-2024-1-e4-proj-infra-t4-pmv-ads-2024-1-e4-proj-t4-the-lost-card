using Application.Contracts.LostCardDatabase;
using Application.FluentResultExtensions;
using Application.Services;
using FluentValidation;
using FluentResults;
using Mediator;

namespace Application.UseCases.PlayerSignIn;

public sealed record PlayerSignInRequest(string Email, string PlainTextPassword) : IRequest<Result<PlayerSignInResponse>>
{
    public class Validator : AbstractValidator<PlayerSignInRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PlainTextPassword).NotEmpty().Length(3, 255);
        }
    }
}


public sealed record PlayerSignInResponse(string Token, string Name, string Id);

public sealed class PlayerSignInRequestHandler : IRequestHandler<PlayerSignInRequest, Result<PlayerSignInResponse>>
{
    private readonly ICryptographyService cryptographyService;
    private readonly ITokenService tokenService;
    private readonly IPlayerRepository playerRepository;

    public PlayerSignInRequestHandler(
        ICryptographyService cryptographyService, 
        ITokenService tokenService, 
        IPlayerRepository playerRepository
    )
    {
        this.cryptographyService = cryptographyService;
        this.tokenService = tokenService;
        this.playerRepository = playerRepository;
    }

    public async ValueTask<Result<PlayerSignInResponse>> Handle(PlayerSignInRequest request, CancellationToken cancellationToken)
    {
        var player = await playerRepository.Find(request.Email, cancellationToken);

        if (player is null)
            return new ApplicationError("Incorrect user or password");

        if (cryptographyService.CompareSaltedSHA512Hash(request.PlainTextPassword, player.PasswordHash, player.PasswordSalt) is false)
            return new ApplicationError("Incorrect user or password");

        var token = tokenService.GetToken(player);

        return new PlayerSignInResponse(token, player.Name, player.Id!.Value.ToString());
    }
}