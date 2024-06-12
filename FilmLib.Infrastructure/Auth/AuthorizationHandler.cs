using System.Security.Claims;
using FilmLib.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace FilmLib.Infrastructure.Auth;

public class AuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(
            c => c.Type == ClaimTypes.Sid);
        if (userId == null || !Guid.TryParse(userId.Value, out var id))
        {
            return;
        }

        var scope = serviceScopeFactory.CreateScope();
        var permissionService = scope.ServiceProvider
            .GetRequiredService<IPermissionsService>();
        var permissions = await permissionService.GetUserPermissions(id);
        
        if (permissions.Intersect(requirement.Permissions).Any())
        {
            context.Succeed(requirement);
        }

    }
}