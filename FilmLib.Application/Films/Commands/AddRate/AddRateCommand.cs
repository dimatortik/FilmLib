using FilmLib.Application.Messaging;

namespace FilmLib.Application.Films.Commands.AddRate;

public record AddRateCommand(
    Guid FilmId,
    decimal VoteValue,
    Guid UserId) : ICommand;
