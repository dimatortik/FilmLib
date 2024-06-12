using FilmLib.Application.Messaging;

namespace FilmLib.Application.Genres.Commands.Create;

public record AddGenreCommand(
    string Title,
    string Description) : ICommand;
