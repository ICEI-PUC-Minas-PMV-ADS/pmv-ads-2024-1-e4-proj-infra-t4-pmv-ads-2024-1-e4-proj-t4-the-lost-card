using FluentResults;

namespace Application.FluentResultExtensions;

public class AuthError : Error
{
    public const string DefaultMessage = "An authorization or authentication error has occured";
    public AuthError() : base(DefaultMessage) { }
}