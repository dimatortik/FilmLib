using FilmLib.Application.Messaging;

namespace FilmLib.Application.Films.Commands.UpdateRate;

public record UpdateRateCommand(
    Guid Id,
    int RateValue,
    Guid UserId) : ICommand;
