using ETicaretAPI.Application.Abstractions.Services.Authentications;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IAuthService : IExternalAuthenticaiton, IInternalAuthentication
    {
        Task PasswordResetAsync(string email);
        Task<bool> VerifyResetToken(string resetToken, string userId);
    }
}
