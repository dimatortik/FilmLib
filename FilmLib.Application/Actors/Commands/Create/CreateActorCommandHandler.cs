using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Actors.Commands.Create;

public class CreateActorCommandHandler(AppDbContext context) : ICommandHandler<CreateActorCommand>
{
    public async Task<Result> Handle(CreateActorCommand request, CancellationToken cancellationToken)
    {
        
        var actorExists = await context.Actors
            .AsNoTracking()
            .AnyAsync(a => a.Name == request.ActorName, cancellationToken);
        if (actorExists)
            return Result.Failure("Actor with the same name already exists.");
        
        var actor = Actor.Create(
            request.ActorImageLink, 
            request.ActorName, 
            request.ActorDescription);
        
        if (actor.IsFailure)
            return Result.Failure(actor.Error);
        
        await context.Actors
            .AddAsync(actor.Value, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
       
        return Result.Success();
    }
}