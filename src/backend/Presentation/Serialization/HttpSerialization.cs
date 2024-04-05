using Application.FluentResultExtensions;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Presentation.Serialization;

public class HttpSerialization
{
    public static IActionResult Serialize<TValue>(Result<TValue> result)
    {
        if (result is { IsSuccess: true })
            return new ObjectResult(result.Value);
        
        return Serialize(result.ToResult());
    }

    public static IActionResult Serialize(Result result)
    {
        var firstError = result.Errors.FirstOrDefault();

        if (firstError is RequestValidationError validationError)
        {
            var validationProblemDetails = new ValidationProblemDetails(validationError.FieldReasonDictionary)
            {
                Detail = firstError.Message,
                Status = 500,
                Title = "Validation error"
            };

            return new ObjectResult(validationProblemDetails) { StatusCode = StatusCodes.Status400BadRequest };
        }

        var problemDetails = new ProblemDetails
        {
            Detail = firstError.Message,
            Status = 500,
            Title = "Unhandled error",
        };

        return new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status500InternalServerError };
    }
}
