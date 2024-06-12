using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FilmLib.Application.Films.Queries.GetById;
using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;
using FilmLib.Persistence;

namespace FilmLib.Application.Films.Queries.GetAll;

public class GetFilmsQueryHandler(AppDbContext context) : IQueryHandler<GetFilmsQuery,PagedList<FilmResponse>>
{


    public async Task<Result<PagedList<FilmResponse>>> Handle(GetFilmsQuery request, CancellationToken cancellationToken)
    { 
        IQueryable<Film> filmQuery = context.Films;
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            filmQuery = filmQuery.Where(f =>
                    f.Title.Contains(request.SearchTerm));
        }
        
        if (request.SortOrder?.ToLower() == "desc")
        {
            filmQuery = filmQuery.OrderByDescending(GetSortProperty(request));
        }
        else
        {
            filmQuery = filmQuery.OrderBy(GetSortProperty(request));
        }

        var filmsResponsesQuery = filmQuery
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
            });
        var films = await PagedList<FilmResponse>.CreateAsync(
            filmsResponsesQuery, 
            request.Page, 
            request.PageSize);
        
        return Result.Success(films);
    }

    private static Expression<Func<Film, object>> GetSortProperty(GetFilmsQuery request) => 
        request.SortColumn?.ToLower() switch
        {
            "rating" => f => f.Rating.RatingValue,
            "title" => f => f.Title,
            "year" => f => f.Year,
            _ => f => f.Rating.RatingValue
        };
}
