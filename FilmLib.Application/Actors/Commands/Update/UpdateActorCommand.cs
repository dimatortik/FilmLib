using FilmLib.Application.Messaging;
using FilmLib.Domain.Models;

namespace FilmLib.Application.Actors.Commands.Update;

public record UpdateActorCommand(
    Guid Id,
    string? ActorImageLink,
    string? Name,
    string? Description) : ICommand;
 