using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;
using Result = CSharpFunctionalExtensions.Result;


namespace FilmLib.Application.FilmActions.Commands.CreateFilm;

public class CreateFilmCommandHandler(AppDbContext context) : ICommandHandler<CreateFilmCommand>
{
    public async Task<Result> Handle(CreateFilmCommand request, CancellationToken cancellationToken)
    {
        var filmExists = await context.Films.AnyAsync(f => f.Title == request.Title, cancellationToken);
        if (filmExists)
        {
            return Result.Failure("Film with the same title already exists.");
        }
        var film = Film.Create(request.TitleImageLink, request.Title, request.Description,
            request.Year, request.Country, request.Director, request.FilmVideoLink);
        if (film.IsFailure)
        {
            return Result.Failure(film.Error);
        }
        await context.Films.AddAsync(film.Value, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}