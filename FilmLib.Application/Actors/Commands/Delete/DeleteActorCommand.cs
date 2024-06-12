using FilmLib.Application.Messaging;

namespace FilmLib.Application.Actors.Commands.Delete;

public record DeleteActorCommand(
    Guid Id) : ICommand;