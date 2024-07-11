namespace ETicaretAPI.Application.Exceptions
{
    public class RefreshTokenExpiredException : Exception
    {
        public RefreshTokenExpiredException() : base("Refresh token has expired.")
        {
        }

        public RefreshTokenExpiredException(string? message) : base(message)
        {
        }

        public RefreshTokenExpiredException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
