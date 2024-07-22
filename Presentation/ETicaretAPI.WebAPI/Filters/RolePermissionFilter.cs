using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace ETicaretAPI.WebAPI.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userName = context.HttpContext.User.Identity?.Name;
            if (!string.IsNullOrEmpty(userName) && userName != "enesk")
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var attribute = descriptor?.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

                var httpAttribute = descriptor?.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

                var httpType = httpAttribute != null ? httpAttribute.HttpMethods.First() : "GET";

                var code = $"{httpType}_{attribute?.ActionType}_{attribute?.Definition.Replace(" ", "")}";

                var hasRole = await _userService.HasRolePermissionToEndpointAsync(userName, code);

                if (hasRole)
                    await next();
                else
                    context.Result = new UnauthorizedResult();
            } else
                await next();
        }
    }
}
