namespace ETicaretAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Token.Token CreateAccessToken(int minute);
        string CreateRefreshToken();
    }
}
