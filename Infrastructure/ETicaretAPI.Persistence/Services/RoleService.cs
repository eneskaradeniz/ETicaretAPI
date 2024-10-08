﻿using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Persistence.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public (object, int) GetAllRoles(int page, int size)
        {
            var query = _roleManager.Roles;

            IQueryable<AppRole>? rolesQuery = null;

            if (page != -1 && size != -1)
                rolesQuery = query.Skip(page * size).Take(size);
            else
                rolesQuery = query;

            return (rolesQuery.Select(r => new { r.Id, r.Name }), query.Count());
        }

        public async Task<(string id, string name)> GetRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return (role.Id, role.Name);
        }

        public async Task<bool> CreateRoleAsync(string name)
        {
            var result = await _roleManager.CreateAsync(new() { Id = Guid.NewGuid().ToString(), Name = name });
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return false;
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> UpdateRole(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return false;
            role.Name = name;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }
}
