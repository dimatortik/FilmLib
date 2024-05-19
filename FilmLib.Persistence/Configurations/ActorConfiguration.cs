using FilmLib.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmLib.Persistence.Configurations;

public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(Actor.MAX_ACTOR_NAME_LENGTH);
        builder.Property(a => a.Description)
            .HasMaxLength(Actor.MAX_ACTOR_DESCRIPTION_LENGTH);
        builder.Property(a => a.ActorImageLink)
            .IsRequired();
        builder.HasMany(a => a.Films)
            .WithMany(f => f.Actors);
    }
}