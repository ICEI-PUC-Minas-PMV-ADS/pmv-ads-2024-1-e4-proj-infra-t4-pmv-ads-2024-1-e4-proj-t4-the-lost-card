﻿using Application.FluentResultExtensions;
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
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation error"
            };

            return new ObjectResult(validationProblemDetails) { StatusCode = StatusCodes.Status400BadRequest };
        }

        if (firstError is AuthError authError)
        {
            var validationProblemDetails = new ProblemDetails()
            {
                Detail = authError.Message,
                Status = StatusCodes.Status401Unauthorized,
                Title = authError.Message
            };

            return new ObjectResult(validationProblemDetails) { StatusCode = StatusCodes.Status401Unauthorized };
        }

        var problemDetails = new ProblemDetails
        {
            Detail = firstError?.Message,
            Status = StatusCodes.Status500InternalServerError,
            Title = "Unhandled error",
        };

        return new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status500InternalServerError };
    }
}