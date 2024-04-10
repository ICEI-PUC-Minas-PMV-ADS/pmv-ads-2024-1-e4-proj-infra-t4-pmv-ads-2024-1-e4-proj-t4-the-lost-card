using FluentResults;

namespace Application.FluentResultExtensions;

public class NotFoundError : Error
{
    public NotFoundError(string message = "not found") : base(message) { }
}
