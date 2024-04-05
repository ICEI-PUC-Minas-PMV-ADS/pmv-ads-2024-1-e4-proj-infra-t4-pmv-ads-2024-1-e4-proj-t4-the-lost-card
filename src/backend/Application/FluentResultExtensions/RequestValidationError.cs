using FluentResults;

namespace Application.Errors;

public class RequestValidationError : Error
{
    public Dictionary<string, string[]> FieldReasonDictionary { get; init; } = default!;
}
