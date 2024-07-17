namespace ETicaretAPI.Application.Exceptions
{
    public class PasswordChangedFailedException : Exception
    {
        public PasswordChangedFailedException() : base("Password could not be changed.")
        {
        }

        public PasswordChangedFailedException(string? message) : base(message)
        {
        }

        public PasswordChangedFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
