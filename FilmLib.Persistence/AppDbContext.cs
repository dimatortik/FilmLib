using FilmLib.Domain.Models;
using FilmLib.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FilmLib.Persistence;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IOptions<AuthorizationOptions> auth
    ) : DbContext(options)
{
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<FilmComment> Comments { get; set; }
    
    public DbSet<RoleEntity> Roles { get; set; }
    
    public DbSet<PermissionEntity> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(auth.Value));
    }
}