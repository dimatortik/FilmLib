using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using FilmLib.Application.Films.Queries.GetById;
using FilmLib.Application.Interfaces;
using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;
using FilmLib.Persistence;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace FilmLib.Application.Films.Queries.GetAll;

public class GetFilmsQueryHandler(AppDbContext context, IDistributedCache cachingService) : IQueryHandler<GetFilmsQuery,PagedList<FilmResponse>>
{
    public async Task<Result<PagedList<FilmResponse>>> Handle(GetFilmsQuery request, CancellationToken cancellationToken)
    {
        var key = GenerateCacheKey(request);
        
        var cachedFilms = await cachingService.GetStringAsync(key, token: cancellationToken);

        if (!string.IsNullOrWhiteSpace(cachedFilms))
        {
            var cachedFilmsResponse = JsonConvert.DeserializeObject<PagedList<FilmResponse>>(cachedFilms,
                new JsonSerializerSettings
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                })!;
            return Result.Success(cachedFilmsResponse);
        }
        
        IQueryable<Film> filmQuery = context.Films;
        
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            filmQuery = filmQuery.Where(f =>
                f.Title.Contains(request.SearchTerm));
        }
        
        if (request.Genres.Any())
        {
            filmQuery = filmQuery.Where(f =>
                f.Genres
                    .Select(g => g.Title)
                    .Intersect(request.Genres)
                    .Any());
        }
        
        filmQuery = request.SortOrder?.ToLower() == "desc" ? 
            filmQuery.OrderByDescending(GetSortProperty(request)) : 
            filmQuery.OrderBy(GetSortProperty(request));

        var filmsResponsesQuery = filmQuery
            .Select(f => new FilmResponse
            {
                Id = f.Id,
                TitleImageLink = f.TitleImageLink,
                Title = f.Title,
                Description = f.Description,
                Views = f.Views,
                Rating = f.RatingObject.RatingValue,
                Year = f.Year,
                Country = f.Country,
                Director = f.Director,
                FilmVideoLink = f.FilmVideoLink
            });
        var films = await PagedList<FilmResponse>.CreateAsync(
            filmsResponsesQuery, 
            request.Page, 
            request.PageSize);

        await cachingService.SetStringAsync(key,
            JsonConvert.SerializeObject(films), token: cancellationToken);
        
        return Result.Success(films);
    }
    
    private static string GenerateCacheKey(GetFilmsQuery request)
    {
        var keyBuilder = new StringBuilder("Films:");
        if (request.Genres.Count != 0)
        {
            keyBuilder.Append($"Genres:{string.Join(",", request.Genres.OrderBy(g => g))};");
        }
        keyBuilder.Append($"Page:{request.Page};");
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            keyBuilder.Append($"Search:{request.SearchTerm};");
        }
        if (!string.IsNullOrWhiteSpace(request.SortColumn))
        {
            keyBuilder.Append($"Sort:{request.SortColumn};Order:{request.SortOrder};");
        }
        return keyBuilder.ToString();
    }


    private static Expression<Func<Film, object>> GetSortProperty(GetFilmsQuery request) => 
        request.SortColumn?.ToLower() switch
        {
            "rating" => f => f.RatingObject.RatingValue,
            "title" => f => f.Title,
            "year" => f => f.Year,
            _ => f => f.RatingObject.RatingValue
        };
}
