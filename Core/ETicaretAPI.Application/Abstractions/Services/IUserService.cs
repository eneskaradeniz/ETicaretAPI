using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Domain.Entities.Identity;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<ListUser> GetAllUsersAsync(int page, int size);
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task<bool> UpdateRefreshTokenAsync(AppUser user, string refreshToken, DateTime accessTokenDate, int addOnAccessTokenDate);
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        Task AssignRoleToUserAsync(string userId, string[] roles);
        Task<string[]> GetRolesToUserAsync(string userId);
    }
}
