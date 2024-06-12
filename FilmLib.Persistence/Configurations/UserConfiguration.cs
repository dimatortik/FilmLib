using FilmLib.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmLib.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(User.MAX_USERNAME_LENGTH);
        builder.Property(u => u.PasswordHash)
            .IsRequired();
        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users);
    }
}