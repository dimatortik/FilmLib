using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace FilmLib.Application.Films.Queries.GetById;

public class GetFilmByIdQueryHandler(AppDbContext context, IDistributedCache cache) : IQueryHandler<GetFilmByIdQuery, FilmResponse>
{
    public async Task<Result<FilmResponse>> Handle(GetFilmByIdQuery request, CancellationToken cancellationToken)
    {
        var key = $"film-{request.Id}";
        
        var cachedFilm = await cache.GetStringAsync(key, token: cancellationToken);
        if (!string.IsNullOrWhiteSpace(cachedFilm))
        {
            var filmic = JsonConvert.DeserializeObject<FilmResponse>(cachedFilm)!;
            return Result.Success(filmic);
        }
        
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
            Rating = film.RatingObject.RatingValue,
            Year = film.Year,
            Country = film.Country,
            Director = film.Director,
            FilmVideoLink = film.FilmVideoLink
        };
        
        await cache.SetStringAsync(key,JsonConvert.SerializeObject(response), token: cancellationToken);
        
        return Result.Success(response);
    }
}
