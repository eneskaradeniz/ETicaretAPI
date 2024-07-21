namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IAuthorizationEndpointService
    {
        Task AssignRoleEndpointAsync(string[] roles, string menuName, string code, Type type);
        Task<List<string>> GetRolesToEndpointAsync(string code, string menu);
    }
}
