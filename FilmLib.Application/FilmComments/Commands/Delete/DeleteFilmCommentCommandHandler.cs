using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;

namespace FilmLib.Application.FilmComments.Commands.Delete;

public class DeleteFilmCommentCommandHandler(AppDbContext context) : ICommandHandler<DeleteFilmCommentCommand>
{
    public async Task<Result> Handle(DeleteFilmCommentCommand request, CancellationToken cancellationToken)
    {
        var filmComment = await context.Comments.FindAsync(request.Id);
        if (filmComment == null)
        {
            return Result.Failure("Film comment not found");
        }

        context.Comments.Remove(filmComment);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}