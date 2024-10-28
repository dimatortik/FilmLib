using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;
using FilmLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilmLib.Application.Genres.Commands.Create;

public class AddGenreCommandHandle(AppDbContext context) : ICommandHandler<AddGenreCommand>
{
    public async Task<Result> Handle(AddGenreCommand request, CancellationToken cancellationToken)
    {
        var genreExist = await context.Genres
            .AnyAsync(g => g.Title == request.Title, cancellationToken: cancellationToken);
       
        if (genreExist)
        {
            return Result.Failure("Genre with the same title already exists.");
        }
        
        var genre = Genre.Create(request.Title,
            request.Description);
        if (genre.IsFailure)
        {
            return Result.Failure(genre.Error);
        }
        
        await context.Genres.AddAsync(genre.Value, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}