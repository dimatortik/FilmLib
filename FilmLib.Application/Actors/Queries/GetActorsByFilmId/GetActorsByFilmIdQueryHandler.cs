using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Actors.Queries.GetActorsByFilmId;

public class GetActorsByFilmIdQueryHandler(AppDbContext context) : IQueryHandler<GetActorsByFilmIdQuery, List<ActorResponse>>
{
    public async Task<Result<List<ActorResponse>>> Handle(GetActorsByFilmIdQuery request, CancellationToken cancellationToken)
    {
        var filmExist = await context.Films
            .AsNoTracking()
            .AnyAsync(f => f.Id == request.FilmId, cancellationToken: cancellationToken);
        if (!filmExist)
        {
            return Result.Failure<List<ActorResponse>>("Film not found.");
        }
        
        var actors = await context.Films
            .AsNoTracking()
            .Where(f => f.Id == request.FilmId)
            .SelectMany(f => f.Actors)
            .Select(a => new ActorResponse
            {
                Id = a.Id,
                ActorImageLink = a.ActorImageLink,
                Name = a.Name,
                Description = a.Description
            })
            .ToListAsync(cancellationToken);
        return Result.Success(actors);
    }
}