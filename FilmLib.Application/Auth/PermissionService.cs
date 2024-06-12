using FilmLib.Application.Interfaces;
using FilmLib.Domain.Enums;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Auth;

public class PermissionService(AppDbContext context): IPermissionsService
{
    public async Task<HashSet<Permission>> GetUserPermissions(Guid userId)
    {
        var roles = await context.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId)
            .Select(u => u.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(r => r)
            .SelectMany(r => r.Permissions)
            .Select(p => (Permission)p.Id)
            .ToHashSet();
    }
    
}