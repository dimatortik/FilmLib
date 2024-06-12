using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Actors.Queries.GetAll;

public class GetActorsQueryHandler(AppDbContext context) : IQueryHandler<GetActorsQuery, List<ActorResponse>>
{
    public async Task<Result<List<ActorResponse>>> Handle(GetActorsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Actor> actorQuery = context.Actors;
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            actorQuery = actorQuery
                .Where(f =>
                    f.Name.Contains(request.SearchTerm))
                .AsNoTracking();

        }
        
        var actors = await actorQuery
            .Select(f => new ActorResponse
            {
                Id = f.Id,
                ActorImageLink = f.ActorImageLink,
                Name = f.Name,
                Description = f.Description
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        return Result.Success(actors);
    }
}