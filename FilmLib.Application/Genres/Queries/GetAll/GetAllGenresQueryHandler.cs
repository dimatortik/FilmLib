using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Genres.Queries.GetAll;

public class GetAllGenresQueryHandler(AppDbContext context) : IQueryHandler<GetAllGenresQuery, List<GenreResponse>>
{
    public async Task<Result<List<GenreResponse>>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Genre> query = context.Genres;
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(g =>
                g.Title.Contains(request.SearchTerm));
        }
        var genreResponses = await query
            .Select(f => new GenreResponse(){
                Id = f.Id,
                Title = f.Title,
                Description = f.Description
            }).ToListAsync(cancellationToken: cancellationToken);
        
        return Result.Success(genreResponses);
    }
}