using ErrorOr;

namespace SimpleTodo.Domain.Errors;

public static class UserErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: $"User.{nameof(NotFound)}",
        description: "The specified user was not found");
}
