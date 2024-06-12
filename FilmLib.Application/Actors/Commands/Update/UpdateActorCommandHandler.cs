using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Actors.Commands.Update;

public class UpdateActorCommandHandler(AppDbContext context) : ICommandHandler<UpdateActorCommand>
{
    public async Task<Result> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
    {
        var actor = await context.Actors.FindAsync(request.Id);
        if (actor == null)
        {
            return Result.Failure("Actor not found.");
        }

        var editedActor = actor.Edit(
            request.ActorImageLink,
            request.Name,
            request.Description);

        if (editedActor.IsFailure)
        {
            return Result.Failure(editedActor.Error);
        }
        
        await context.Actors.ExecuteUpdateAsync( setters => setters.
                SetProperty(p => p.ActorImageLink, p => request.ActorImageLink)
                .SetProperty(p => p.Name, p => request.Name)
                .SetProperty(p => p.Description, p => request.Description), cancellationToken: cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
