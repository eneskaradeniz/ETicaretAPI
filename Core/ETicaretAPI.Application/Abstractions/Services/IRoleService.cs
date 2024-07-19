﻿namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IRoleService
    {
        (object, int) GetAllRoles(int page, int size);
        Task<(string id, string name)> GetRoleById(string id);
        Task<bool> CreateRoleAsync(string name);
        Task<bool> DeleteRoleAsync(string id);
        Task<bool> UpdateRole(string id, string name);
    }
}
