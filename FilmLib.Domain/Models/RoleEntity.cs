using FilmLib.Domain.Models;

namespace FilmLib.Persistence;

public class RoleEntity
{
    public const int MAX_ROLE_NAME_LENGTH = 10;
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<PermissionEntity> Permissions { get; set; } = [];

    public ICollection<User> Users { get; set; } = [];
}