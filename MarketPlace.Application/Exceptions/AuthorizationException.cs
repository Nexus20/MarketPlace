namespace MarketPlace.Application.Exceptions;

[Serializable]
public class AuthorizationException : ApplicationException
{
    public AuthorizationException()
    {
    }

    public AuthorizationException(string message) : base(message)
    {
    }

    public AuthorizationException(string message, Exception? innerException) : base(message, innerException)
    {
    }
}