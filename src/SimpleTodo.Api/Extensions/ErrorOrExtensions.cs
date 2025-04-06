using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace SimpleTodo.Api.Extensions;

public static class ErrorOrExtensions
{
    /// <summary>
    /// Converts a list of <see cref="Error"/> to an <see cref="IActionResult"/>.
    /// </summary>
    /// <param name="errors">The list of errors.</param>
    /// <returns>An <see cref="IActionResult"/> with the correctly mapped properties to return to the client.</returns>
    public static IActionResult ToProblemDetails(this List<Error> errors)
    {
        var first = errors.First();
        var statusCode = first.Type switch
        {
            ErrorType.Validation => StatusCodes.Status422UnprocessableEntity,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = first.Description,
            Detail = errors.Count > 1
                ? "Multiple errors occured. See 'errors' for more details. "
                : first.Description,
            Extensions =
            {
                ["errorCode"] = first.Code,
                ["errors"] = errors.Select(e => new { e.Code, e.Description })
            }
        };

        return new ObjectResult(problemDetails);
    }
}
