using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Genres.Commands.Delete;

public class DeleteGenreCommandHandler(AppDbContext context) : ICommandHandler<DeleteGenreCommand>
{
    public async Task<Result> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genreExist = await context.Genres
            .AnyAsync(g => g.Id == request.Id, cancellationToken: cancellationToken);
        if (!genreExist)
        {
            return Result.Failure("Genre not found.");
        }
        
        await context.Genres.Where(f => f.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);
        return Result.Success();
    }
}