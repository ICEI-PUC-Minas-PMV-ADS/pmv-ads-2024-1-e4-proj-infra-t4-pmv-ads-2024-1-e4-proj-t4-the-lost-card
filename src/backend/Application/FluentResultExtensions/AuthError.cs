using FluentResults;

namespace Application.FluentResultExtensions;

public class AuthError : Error
{
    public AuthError() : base("An authorization or authentication error has occured") { }
}