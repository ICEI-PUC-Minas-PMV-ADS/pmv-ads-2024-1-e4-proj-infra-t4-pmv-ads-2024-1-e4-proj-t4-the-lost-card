using Domain.Entities;

namespace Application.Services;

public interface ITokenService
{
    string GetToken(Player user);
}
