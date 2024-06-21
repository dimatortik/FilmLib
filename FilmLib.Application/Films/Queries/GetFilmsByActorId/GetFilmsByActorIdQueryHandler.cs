using CSharpFunctionalExtensions;
using FilmLib.Application.Films.Queries.GetById;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Films.Queries.GetFilmsByActorId;

public class GetFilmsByActorIdQueryHandler(AppDbContext context) : IQueryHandler<GetFilmsByActorIdQuery, List<FilmResponse>>
{
    public async Task<Result<List<FilmResponse>>> Handle(GetFilmsByActorIdQuery request, CancellationToken cancellationToken)
    {
        var actorExist = await context.Actors
            .AnyAsync(a => a.Id == request.ActorId, cancellationToken: cancellationToken);
        if (!actorExist)
            return Result.Failure<List<FilmResponse>>("Actor not found.");
        var responses = await context.Actors
            .AsNoTracking()
            .Where(a => a.Id == request.ActorId)
            .SelectMany(a => a.Films)
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