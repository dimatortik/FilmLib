using CSharpFunctionalExtensions;
using FilmLib.Application.Films.Queries.GetById;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Films.Queries.GetFilmsByGenreId;

public class GetFilmsByGenreIdQueryHandler(AppDbContext context) : IQueryHandler<GetFilmsByGenreIdQuery, List<FilmResponse>>
{
    public async Task<Result<List<FilmResponse>>> Handle(GetFilmsByGenreIdQuery request, CancellationToken cancellationToken)
    {
        var actorExist = await context.Genres
            .AnyAsync(g => g.Id == request.Id, cancellationToken: cancellationToken);
        if (!actorExist)
            return Result.Failure<List<FilmResponse>>("Genre not found.");
        var responses = await context.Genres
            .AsNoTracking()
            .Where(g => g.Id == request.Id)
            .SelectMany(g => g.Films)
            .Select(f => new FilmResponse
            {
                Id = f.Id,
                TitleImageLink = f.TitleImageLink,
                Title = f.Title,
                Description = f.Description,
                Views = f.Views,
                Rating = f.Rating.RatingValue,
                Year = f.Year,
                Country = f.Country,
                Director = f.Director,
                FilmVideoLink = f.FilmVideoLink
            })
            .ToListAsync(cancellationToken);
        return Result.Success(responses);
    }
}