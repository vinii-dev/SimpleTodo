namespace SimpleTodo.Application.Exceptions.Auth;

public class UsernameAlreadyInUseException : Exception
{
    private const string MESSAGE = "The specified username is already in use. ";
    public UsernameAlreadyInUseException() : base(MESSAGE)
    {
    }
    public UsernameAlreadyInUseException(Exception innerException) : base(MESSAGE, innerException)
    {
    }
}
