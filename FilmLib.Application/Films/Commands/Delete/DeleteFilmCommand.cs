using FilmLib.Application.Messaging;

namespace FilmLib.Application.Films.Commands.Delete;

public record DeleteFilmCommand(Guid Id) : ICommand;
