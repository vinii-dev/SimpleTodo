namespace SimpleTodo.Application.Exceptions.User;

public class UserNotFoundException : Exception
{
    private const string MESSAGE = "The specified user was not found. ";

    public UserNotFoundException() : base()
    {
    }

    public UserNotFoundException(Exception innerException) : base(MESSAGE, innerException)
    {
    }
}
