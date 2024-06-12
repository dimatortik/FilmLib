using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Genres.Queries.GetGenresByFilmId;

public class GetGenresByFilmIdQueryHandler(AppDbContext context) : IQueryHandler<GetGenresByFilmIdQuery, List<GenreResponse>>
{
    public async Task<Result<List<GenreResponse>>> Handle(GetGenresByFilmIdQuery request, CancellationToken cancellationToken)
    {
        var filmExist = await context.Films
            .AsNoTracking()
            .AnyAsync(f => f.Id == request.FilmId, cancellationToken: cancellationToken);
        if (!filmExist)
        {
            return Result.Failure<List<GenreResponse>>("Film not found.");
        }
        
        var genres = await context.Films
            .AsNoTracking()
            .Where(f => f.Id == request.FilmId)
            .SelectMany(f => f.Genres)
            .Select(g => new GenreResponse
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description
            })
            .ToListAsync(cancellationToken);
        
        return Result.Success(genres);
    }
}