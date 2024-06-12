using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;

namespace FilmLib.Application.Genres.Queries.GetById;

public class GetGenreQueryHandler(AppDbContext context) : IQueryHandler<GetGenreQuery, GenreResponse>
{
    public async Task<Result<GenreResponse>> Handle(GetGenreQuery request, CancellationToken cancellationToken)
    {
        var genre = await context.Genres
            .FindAsync(request.Id);
        if (genre == null)
            return Result.Failure<GenreResponse>("Genre not found.");
        
        var response = new GenreResponse
        {
            Id = genre.Id,
            Title = genre.Title,
            Description = genre.Description
        };
        
        return Result.Success(response);
    }
}