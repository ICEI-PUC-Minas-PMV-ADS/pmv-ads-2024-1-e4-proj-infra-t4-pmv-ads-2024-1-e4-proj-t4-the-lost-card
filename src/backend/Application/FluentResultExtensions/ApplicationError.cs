using FluentResults;

namespace Application.FluentResultExtensions;

public class ApplicationError : Error
{
    public ApplicationError(string message) : base(message) {  }
}
