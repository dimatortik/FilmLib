using FilmLib.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace FilmLib.Infrastructure.Auth;

public class PermissionRequirement(Permission[] permissions) : IAuthorizationRequirement
{
    public Permission[] Permissions { get; set; } = permissions;
}