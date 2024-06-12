using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Actors.Commands.Delete;

public class DeleteActorCommandHandler(AppDbContext context) : ICommandHandler<DeleteActorCommand>
{

    public async Task<Result> Handle(DeleteActorCommand request, CancellationToken cancellationToken)
    {
        var actor = await context.Actors
            .FindAsync(request.Id);
        if (actor == null)
        {
            return Result.Failure("Actor not found.");
        }
        await context.Actors.Where(a => request.Id == a.Id)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);
        return await Task.FromResult(Result.Success());
    }
}