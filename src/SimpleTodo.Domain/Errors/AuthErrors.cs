using ErrorOr;

namespace SimpleTodo.Domain.Errors;

public static class AuthErrors
{
    public static readonly Error InvalidCredentials = Error.Unauthorized(
        code: $"Auth.{nameof(InvalidCredentials)}",
        description: "The provided username or password is incorrect.");

    public static readonly Error UsernameAlreadyInUse = Error.Conflict(
        code: $"Auth.{nameof(UsernameAlreadyInUse)}",
        description: "The specified username is already in use.");
}
