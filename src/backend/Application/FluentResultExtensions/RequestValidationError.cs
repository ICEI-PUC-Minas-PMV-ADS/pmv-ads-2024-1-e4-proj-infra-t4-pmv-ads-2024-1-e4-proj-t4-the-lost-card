using FluentResults;

namespace Application.FluentResultExtensions;

public class RequestValidationError : Error
{
    public Dictionary<string, string[]> FieldReasonDictionary { get; init; } = default!;
}
