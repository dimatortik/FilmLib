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
        builder.Property(f => f.Year);
        builder.Property(f => f.FilmVideoLink);
        builder.Property(f => f.Country);
        builder.Property(f => f.Director);
        builder.Property(f => f.Views);
        builder.ComplexProperty(f => f.RatingObject, ratingBuilder =>
        {
            ratingBuilder.Property(r => r.RatingValue).HasColumnName("Rating");
            ratingBuilder.Property(r => r.NumberOfVotes).HasColumnName("NumberOfVotes");
        });
        builder.HasMany(f => f.Actors)
            .WithMany(a => a.Films);
        builder.HasMany(f => f.Genres)
            .WithMany(g => g.Films);
        builder.HasMany(f => f.FilmComments)
            .WithOne(c => c.Film);

    }
}