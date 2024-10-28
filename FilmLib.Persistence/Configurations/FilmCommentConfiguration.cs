using FilmLib.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmLib.Persistence.Configurations;

public class FilmCommentConfiguration : IEntityTypeConfiguration<FilmComment>
{
    public void Configure(EntityTypeBuilder<FilmComment> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Body)
            .IsRequired()
            .HasMaxLength(FilmComment.MAX_FILM_COMMENT_LENGTH);
        builder.HasOne(c => c.User);
        builder.HasOne(c => c.Film);
    }
}