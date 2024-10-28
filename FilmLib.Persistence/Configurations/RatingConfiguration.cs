using FilmLib.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmLib.Persistence.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.RatingNumber)
            .IsRequired();
        builder.Property(r => r.FilmId);
        builder.Property(r => r.UserId);
        builder.Property(r => r.Date).
            HasColumnType("timestamp without time zone");
        builder.HasOne(r => r.User);
        builder.HasOne(r => r.Film);
    }
}