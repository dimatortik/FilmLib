using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Films.Commands.Delete;

public class DeleteFilmCommandHandler(AppDbContext context) : ICommandHandler<DeleteFilmCommand>

{
    public async Task<Result> Handle(DeleteFilmCommand request, CancellationToken cancellationToken)
    {
        var film = await context.Films.FindAsync(request.Id);
        if (film == null)
        {
            return Result.Failure("Film not found.");
        }

        await context.Films.Where(f => request.Id == f.Id)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);
        return Result.Success();

    }
}