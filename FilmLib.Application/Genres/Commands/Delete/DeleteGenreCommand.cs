using FilmLib.Application.Messaging;

namespace FilmLib.Application.Genres.Commands.Delete;

public record DeleteGenreCommand(int Id) : ICommand;
