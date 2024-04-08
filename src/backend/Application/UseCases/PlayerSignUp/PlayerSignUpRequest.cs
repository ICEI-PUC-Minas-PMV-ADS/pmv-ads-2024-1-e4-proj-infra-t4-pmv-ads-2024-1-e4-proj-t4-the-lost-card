using Application.Contracts.LostCardDatabase;
using Application.Services;
using Domain.Entities;
using FluentResults;
using Mediator;
using FluentValidation;

namespace Application.UseCases.PlayerSignUp;

public sealed record PlayerSignUpRequest(string Name, string Email, string PlainTextPassword) : IRequest<Result<PlayerSignUpResponse>>
{
    public sealed class Validator : AbstractValidator<PlayerSignUpRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 255);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PlainTextPassword).NotEmpty().Length(3, 255);
        }
    }
}

public sealed record PlayerSignUpResponse(string Id);

public sealed class PlayerSignUpRequestHandler : IRequestHandler<PlayerSignUpRequest, Result<PlayerSignUpResponse>>
{
    private readonly IPlayerRepository playerRepository;
    private readonly ILostCardDbUnitOfWork unitOfWork;
    private readonly ICryptographyService cryptographyService;

    public PlayerSignUpRequestHandler(ICryptographyService cryptographyService, IPlayerRepository playerRepository, ILostCardDbUnitOfWork unitOfWork)
    {
        this.cryptographyService = cryptographyService;
        this.playerRepository = playerRepository;
        this.unitOfWork = unitOfWork;
    }

    public async ValueTask<Result<PlayerSignUpResponse>> Handle(PlayerSignUpRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await playerRepository.Find(request.Email, cancellationToken);

        if (existingUser is not null)
            return new Error("User already registred");

        var (passwordHash, passwordSalt) = cryptographyService.GenerateSaltedSHA512Hash(request.PlainTextPassword);

        var player = new Player
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await playerRepository.Create(player, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new PlayerSignUpResponse(player.Id!.ToString()!).ToResult();
    }
}