using FilmLib.Application.Messaging;

namespace FilmLib.Application.Genres.Commands.Update;

public record EditGenreCommand(
    int Id,
    string Title,
    string Description) : ICommand;
