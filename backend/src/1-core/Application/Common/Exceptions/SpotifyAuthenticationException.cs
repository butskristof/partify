namespace Partify.Application.Common.Exceptions;

public sealed class SpotifyAuthenticationException : Exception
{
    public SpotifyAuthenticationException() { }

    public SpotifyAuthenticationException(string? message)
        : base(message) { }

    public SpotifyAuthenticationException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
