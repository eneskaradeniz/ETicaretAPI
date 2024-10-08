﻿using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> _userManager;
        readonly IEndpointReadRepository _endpointReadRepository;

        public UserService(UserManager<AppUser> userManager, IEndpointReadRepository endpointReadRepository)
        {
            _userManager = userManager;
            _endpointReadRepository = endpointReadRepository;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                NameSurname = model.NameSurname
            }, model.Password);

            CreateUserResponse response = new() { Succeeded = result.Succeeded };
            if (result.Succeeded)
                response.Message = "User created successfully.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}";

            return response;
        }

        public async Task<bool> UpdateRefreshTokenAsync(AppUser user, string refreshToken, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = accessTokenDate.AddMinutes(addOnAccessTokenDate);
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new UserNotFoundException();

            resetToken = resetToken.UrlDecode();
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (result.Succeeded)
                await _userManager.UpdateSecurityStampAsync(user);
            else
                throw new PasswordChangedFailedException();
        }

        public async Task<ListUser> GetAllUsersAsync(int page, int size)
        {
            var query = _userManager.Users;

            var datas = await query
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            var users = datas.Select(user => new
            {
                Id = user.Id,
                Email = user.Email,
                NameSurname = user.NameSurname,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName,
            }).ToList();

            var totalUserCount = await query.CountAsync();

            return new()
            {
                TotalUserCount = totalUserCount,
                Users = users
            };
        }

        public async Task AssignRoleToUserAsync(string userId, string[] roles)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                await _userManager.AddToRolesAsync(user, roles);
            }
        }

        public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
        {
            AppUser user = await _userManager.FindByIdAsync(userIdOrName);
            user ??= await _userManager.FindByNameAsync(userIdOrName);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }
            return [];
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string userName, string code)
        {
            var userRoles = await GetRolesToUserAsync(userName);
            if (!userRoles.Any())
                return false;

            var endpoint = await _endpointReadRepository.Table
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Code == code);
            if (endpoint == null)
                return false;

            var hasRole = false;

            var endpointRoles = endpoint.Roles.Select(r => r.Name);

            foreach (var userRole in userRoles)
                foreach (var endpointRole in endpointRoles)
                    if (userRole == endpointRole)
                        return true;

            return false;
        }
    }
}
