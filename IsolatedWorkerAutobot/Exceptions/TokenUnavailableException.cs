namespace IsolatedWorkerAutobot.Exceptions;

public class TokenUnavailableException : Exception
{
    public TokenUnavailableException(string? message) : base(message)
    {
    }
}
