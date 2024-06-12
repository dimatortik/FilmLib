using FilmLib.Domain.Enums;

namespace FilmLib.Application.Interfaces;

public interface IPermissionsService
{
    Task<HashSet<Permission>> GetUserPermissions(Guid userId);
}