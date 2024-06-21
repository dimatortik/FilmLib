using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;
using FilmLib.Persistence;

namespace FilmLib.Application.FilmComments.Commands.Create;

public class CreateFilmCommentCommandHandler(AppDbContext context ) : ICommandHandler<CreateFilmCommentCommand>
{
    public async Task<Result> Handle(CreateFilmCommentCommand request, CancellationToken cancellationToken)
    {
        var filmComment = FilmComment.Create(
            request.Body, 
            request.UserId, 
            request.FilmId);
        if (filmComment.IsFailure)
        {
            return Result.Failure(filmComment.Error);
        }

        await context.Comments.AddAsync(filmComment.Value, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}