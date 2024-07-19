using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Enums;
using ETicaretAPI.Application.Features.Commands.Role.CreateRole;
using ETicaretAPI.Application.Features.Commands.Role.DeleteRole;
using ETicaretAPI.Application.Features.Commands.Role.UpdateRole;
using ETicaretAPI.Application.Features.Queries.Role.GetRoleById;
using ETicaretAPI.Application.Features.Queries.Role.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class RolesController : ControllerBase
    {
        readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> GetRoles([FromQuery] GetRolesQueryRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Role By Id", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> GetRoleById([FromRoute] GetRoleByIdQueryRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost()]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Create Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> DeleteRole([FromRoute] DeleteRoleCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("{Id}")]
        [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Update Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> UpdateRole([FromBody, FromRoute] UpdateRoleCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
