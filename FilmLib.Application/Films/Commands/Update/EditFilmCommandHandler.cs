using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Films.Commands.Update;

public class EditFilmCommandHandler(AppDbContext context) : ICommandHandler<EditFilmCommand>
{

    public async Task<Result> Handle(EditFilmCommand request, CancellationToken cancellationToken)
    {
        var film = await context.Films.FindAsync(request.Id);
        if (film == null)
        {
            return Result.Failure("Film not found.");
        }
        
        var editedFilm = film.Edit(
            request.TitleImageLink, 
            request.Title, 
            request.Description, 
            request.Year, 
            request.Country, 
            request.Director, 
            request.FilmVideoLink);

        if (editedFilm.IsFailure)
        {
            return Result.Failure(editedFilm.Error);
        }
        
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}