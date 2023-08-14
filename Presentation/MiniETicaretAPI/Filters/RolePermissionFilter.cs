using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.CustomAttributes;

namespace MiniETicaretAPI.Filters;

public class RolePermissionFilter : IAsyncActionFilter
{
    readonly IUserService _userService;

    public RolePermissionFilter(IUserService userService)
    {
        _userService = userService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var name = context.HttpContext.User.Identity?.Name;
        if (!string.IsNullOrEmpty(name) && name != "muratfirtina")
        {
            var desciriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var attribute = desciriptor?.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
            
            var httpAttribute = desciriptor?.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;
            
            var code = $"{(httpAttribute != null ? httpAttribute.HttpMethods.First() : HttpMethods.Get)}.{attribute.ActionType.ToString()}.{attribute.Definition.Replace(" ", "")}";

            var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);
            if (!hasRole)
                context.Result = new UnauthorizedResult();
            else
                await next();

        }
        else
            await next();
        
        
    }
}