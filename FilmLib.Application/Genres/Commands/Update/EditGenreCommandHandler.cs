using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Genres.Commands.Update;

public class EditGenreCommandHandler(AppDbContext context) : ICommandHandler<EditGenreCommand>
{
    public async Task<Result> Handle(EditGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await context.Genres
            .FindAsync(request.Id);
        if (genre == null)
            return Result.Failure("Genre not found.");
        var editedGenre = genre.Edit(request.Title, request.Description);
        if (editedGenre.IsFailure)
            return Result.Failure(editedGenre.Error);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}