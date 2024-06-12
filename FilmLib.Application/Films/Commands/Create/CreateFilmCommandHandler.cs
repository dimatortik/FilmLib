using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;


namespace FilmLib.Application.Films.Commands.Create;

public class CreateFilmCommandHandler(AppDbContext context) : ICommandHandler<CreateFilmCommand>
{
    public async Task<Result> Handle(CreateFilmCommand request, CancellationToken cancellationToken)
    {
        var filmExists = await context.Films.AnyAsync(f => f.Title == request.Title, cancellationToken);
        if (filmExists)
        {
            return Result.Failure("Film with the same title already exists.");
        }

        var film = Film.Create(
            request.TitleImageLink, 
            request.Title, 
            request.Description,
            request.Year, 
            request.Country, 
            request.Director, 
            request.FilmVideoLink);

        var actors = await context.Actors
            .Where(a => request.Actors.Contains(a.Id))
            .ToListAsync(cancellationToken);
        
        if (actors.Count != request.Actors.Count)
        {
            return Result.Failure("Some actors do not exist.");
        }
        
        var genres = await context.Genres
            .Where(g => request.Genres.Contains(g.Id))
            .ToListAsync(cancellationToken);
        
        if (genres.Count != request.Genres.Count)
        {
            return Result.Failure("Some genres do not exist.");
        }
        
        film.Value.AddActors(actors);
        film.Value.AddGenres(genres);

        if (film.IsFailure)
        {
            return Result.Failure(film.Error);
        }

        await context.Films.AddAsync(film.Value, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}