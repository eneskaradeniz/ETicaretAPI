using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Domain.Entities.Identity;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task<bool> UpdateRefreshToken(AppUser user, string refreshToken, DateTime accessTokenDate, int addOnAccessTokenDate);
    }
}
