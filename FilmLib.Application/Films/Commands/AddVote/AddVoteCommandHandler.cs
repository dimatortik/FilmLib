using CSharpFunctionalExtensions;
using FilmLib.Application.Messaging;
using FilmLib.Persistence;

namespace FilmLib.Application.Films.Commands.AddVote;

public class AddVoteCommandHandler(AppDbContext context) : ICommandHandler<AddVoteCommand>
{
    public async Task<Result> Handle(AddVoteCommand request, CancellationToken cancellationToken)
    {
        var film = await context.Films
            .FindAsync(request.FilmId);
        if (film == null)
            return Result.Failure("Film not found.");
        var result = film.AddVote(request.VoteValue);
        if (result.IsFailure)
            return Result.Failure(result.Error);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}