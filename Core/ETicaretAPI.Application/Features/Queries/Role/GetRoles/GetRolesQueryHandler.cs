using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, GetRolesQueryResponse>
    {
        readonly IRoleService _roleService;

        public GetRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRolesQueryResponse> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
        {
            (object datas, int totalCount) = _roleService.GetAllRoles(request.Page, request.Size);
            return new()
            {
                Datas = datas,
                TotalCount = totalCount
            };
        }
    }
}
