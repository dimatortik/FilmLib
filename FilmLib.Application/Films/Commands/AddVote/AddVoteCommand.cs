using FilmLib.Application.Messaging;

namespace FilmLib.Application.Films.Commands.AddVote;

public record AddVoteCommand(
    Guid FilmId,
    int VoteValue) : ICommand;
