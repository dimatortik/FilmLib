using FilmLib.Application.Messaging;

namespace FilmLib.Application.Actors.Commands.Create;

public record CreateActorCommand(
    string ActorImageLink,
    string ActorName,
    string ActorDescription) : ICommand;

