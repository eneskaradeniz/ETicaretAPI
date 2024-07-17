using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs.Token;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.Product.UpdateProduct;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;
        readonly IUserService _userService;
        readonly ILogger<UpdateProductCommandHandler> _logger;
        readonly IMailService _mailService;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler, IUserService userService, ILogger<UpdateProductCommandHandler> logger, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _userService = userService;
            _logger = logger;
            _mailService = mailService;
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
            _logger.LogInformation("LoginAsync called.");
            var user = await _userManager.FindByNameAsync(usernameOrEmail);
            user ??= await _userManager.FindByEmailAsync(password);
            if (user == null)
                throw new UserNotFoundException();

            var result = await _signInManager.PasswordSignInAsync(user, password, true, false);
            if (!result.Succeeded)
                throw new InvalidPasswordException();

            var token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
            await _userService.UpdateRefreshTokenAsync(user, token.RefreshToken, token.Expiration, 5);

            return token;
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken) ?? throw new UserNotFoundException();
            if (user.RefreshTokenEndDate < DateTime.Now)
                throw new RefreshTokenExpiredException();

            var token = _tokenHandler.CreateAccessToken(5, user);
            await _userService.UpdateRefreshTokenAsync(user, token.RefreshToken, token.Expiration, 5);
            return token;
        }

        public async Task PasswordResetAsync(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new UserNotFoundException();

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            resetToken = resetToken.UrlEncode();

            await _mailService.SendPasswordResetMailAsync(email, user.Id, resetToken);
        }

        public async Task<bool> VerifyResetToken(string resetToken, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new UserNotFoundException();

            resetToken = resetToken.UrlDecode();

            return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
        }
    }
}
