using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;

namespace FilmLib.Application.Films.Queries.GetById;

public class GetFilmByIdQueryHandler(AppDbContext context) : IQueryHandler<GetFilmByIdQuery, FilmResponse>
{
    public async Task<Result<FilmResponse>> Handle(GetFilmByIdQuery request, CancellationToken cancellationToken)
    {
        var film = await context.Films
            .FindAsync(request.Id);
        if (film == null)
        {
            return Result.Failure<FilmResponse>("Film not found.");
        }
        
        var response = new FilmResponse
        {
            Id = film.Id,
            TitleImageLink = film.TitleImageLink,
            Title = film.Title,
            Description = film.Description,
            Views = film.Views,
            Rating = film.Rating.RatingValue,
            Year = film.Year,
            Country = film.Country,
            Director = film.Director,
        };
        
        return Result.Success(response);
    }
}
