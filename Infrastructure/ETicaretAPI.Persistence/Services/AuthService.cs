using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs.Token;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IUserService _userService;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _userService = userService;
        }

        public Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
        {
            throw new NotImplementedException();
        }

        public Task GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            throw new NotImplementedException();
        }

        public async Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
        {
            var user = await _userManager.FindByNameAsync(usernameOrEmail);
            user ??= await _userManager.FindByEmailAsync(password);
            if (user == null)
                throw new UserNotFoundException();

            var result = await _signInManager.PasswordSignInAsync(user, password, true, false);
            if (!result.Succeeded)
                throw new InvalidPasswordException();

            var token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
            await _userService.UpdateRefreshToken(user, token.RefreshToken, token.Expiration, 5);

            return token;
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {            
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (user == null)
                throw new UserNotFoundException();

            if (user.RefreshTokenEndDate < DateTime.Now)
                throw new RefreshTokenExpiredException();

            var token = _tokenHandler.CreateAccessToken(5);
            await _userService.UpdateRefreshToken(user, token.RefreshToken, token.Expiration, 5);
            return token;
        }
    }
}
