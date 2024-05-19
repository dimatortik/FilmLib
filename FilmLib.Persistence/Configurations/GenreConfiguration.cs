using FilmLib.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmLib.Persistence.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(Genre.MAX_GENRE_TITLE_LENGTH);
        builder.Property(g => g.Description);
        builder.HasMany(g => g.Films)
            .WithMany(f => f.Genres);
    }
}