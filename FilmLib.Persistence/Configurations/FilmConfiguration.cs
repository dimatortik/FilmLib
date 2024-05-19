using FilmLib.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmLib.Persistence.Configurations;

public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Title)
            .IsRequired()
            .HasMaxLength(Film.MAX_FILM_TITLE_LENGTH);
        builder.Property(f => f.Description)
            .HasMaxLength(Film.MAX_FILM_DESCRIPTION_LENGTH);
        builder.Property(f => f.TitleImageLink)
            .HasMaxLength(Film.MAX_FILM_DESCRIPTION_LENGTH);
        builder.Property(f => f.FilmVideoLink);
        builder.HasMany(f => f.Actors)
            .WithMany(a => a.Films);
        builder.HasMany(f => f.Genres)
            .WithMany(g => g.Films);
        builder.HasMany(f => f.FilmComments)
            .WithOne(c => c.Film);

    }
}