using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.FilmActions.Queries.GetFilms;

public class GetFilmsQueryHandler(AppDbContext context) : IQueryHandler<GetFilmsQuery, List<Film>>
{


    public async Task<Result<List<Film>>> Handle(GetFilmsQuery request, CancellationToken cancellationToken)
    {
        var films =  await context.Films.ToListAsync(cancellationToken: cancellationToken);
        return Result.Success(films);
    }
}
