using CSharpFunctionalExtensions;
using FilmLib.Application.Actors.Queries.GetAll;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Actors.Queries.GetById;

public class GetActorByIdQueryHandler(AppDbContext context) : IQueryHandler<GetActorByIdQuery, ActorResponse>
{

    public async Task<Result<ActorResponse>> Handle(GetActorByIdQuery request, CancellationToken cancellationToken)
    {
        var actor = await context.Actors
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);

        if (actor == null)
            return Result.Failure<ActorResponse>("Actor not found");

        var actorResponse = new ActorResponse
        {
            Id = actor.Id,
            ActorImageLink = actor.ActorImageLink,
            Name = actor.Name,
            Description = actor.Description
        };

        return Result.Success(actorResponse);
    }
}