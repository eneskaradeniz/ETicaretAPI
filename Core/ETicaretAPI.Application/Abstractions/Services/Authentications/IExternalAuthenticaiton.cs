namespace ETicaretAPI.Application.Abstractions.Services.Authentications
{
    public interface IExternalAuthenticaiton
    {
        Task<DTOs.Token.Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime);
        Task GoogleLoginAsync(string idToken, int accessTokenLifeTime);
    }
}
