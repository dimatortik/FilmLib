using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace FilmLib.Application.Films.Commands.Delete;

public class DeleteFilmCommandHandler(AppDbContext context, IDistributedCache cache) : ICommandHandler<DeleteFilmCommand>

{
    public async Task<Result> Handle(DeleteFilmCommand request, CancellationToken cancellationToken)
    {
        var key = $"film-{request.Id}";

        var film = await context.Films.FindAsync(request.Id);
        if (film == null)
        {
            return Result.Failure("Film not found.");
        }

        await context.Films.Where(f => request.Id == f.Id)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);
        
        await cache.RemoveAsync(key, cancellationToken);
        
        return Result.Success();

    }
}